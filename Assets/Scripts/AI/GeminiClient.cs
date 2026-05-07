using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class GeminiClient : MonoBehaviour
{
    [Header("API Settings")]
    [SerializeField] private string url =
        "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent";

    [Header("Prompt Settings")]
    [SerializeField] private PersonaProvider personaProvider;

    private string apiKey = "";

    private void Awake()
    {
        ConfigLoader.Load();
        apiKey = ConfigLoader.GeminiApiKey;
    }

    public async Task<OperationResult<string>> GenerateReplyAsync(string userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput))
        {
            ProjectLogger.Warning("LLM 입력값이 비어 있습니다.");
            return OperationResult<string>.Fail(
                ConversationErrorType.EmptySTTResult,
                "입력이 비어 있습니다."
            );
        }

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            ProjectLogger.Error("Gemini API Key가 설정되지 않았습니다. config.json을 확인하세요.");
            return OperationResult<string>.Fail(
                ConversationErrorType.LLMRequestFailed,
                "Gemini API Key가 설정되지 않았습니다."
            );
        }

        string persona = personaProvider != null ? personaProvider.GetPersona() : "";
        ProjectLogger.LLM("응답 생성 요청");

        string finalUrl = $"{url}?key={apiKey}";
        string jsonBody = BuildRequestJson(persona, userInput);
        byte[] postData = Encoding.UTF8.GetBytes(jsonBody);

        using (UnityWebRequest request = new UnityWebRequest(finalUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                ProjectLogger.Error($"LLM 요청 실패: {request.responseCode} / {request.error}");
                ProjectLogger.Error($"서버 응답: {request.downloadHandler.text}");

                return OperationResult<string>.Fail(
                    ConversationErrorType.LLMRequestFailed,
                    SystemMessageProvider.GetErrorMessage(ConversationErrorType.LLMRequestFailed)
                );
            }

            var parseResult = ParseResponse(request.downloadHandler.text);

            if (parseResult.IsSuccess)
            {
                ProjectLogger.LLM($"응답 완료: {parseResult.Data}");
            }

            return parseResult;
        }
    }

    private string BuildRequestJson(string persona, string userInput)
    {
        string safePersona = EscapeJson(persona);
        string safeUserInput = EscapeJson(userInput);

        return
            "{"
            + "\"system_instruction\": {"
                + "\"parts\": [{\"text\": \"" + safePersona + "\"}]"
            + "},"
            + "\"contents\": [{"
                + "\"role\": \"user\","
                + "\"parts\": [{\"text\": \"" + safeUserInput + "\"}]"
            + "}]"
            + "}";
    }

    private OperationResult<string> ParseResponse(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            ProjectLogger.Error("응답 JSON이 비어 있습니다.");
            return OperationResult<string>.Fail(
                ConversationErrorType.EmptyLLMResponse,
                SystemMessageProvider.GetErrorMessage(ConversationErrorType.EmptyLLMResponse)
            );
        }

        try
        {
            var response = JsonConvert.DeserializeObject<GeminiGenerateContentResponse>(json);

            if (response == null)
            {
                ProjectLogger.Error("응답 역직렬화에 실패했습니다.");
                return OperationResult<string>.Fail(
                    ConversationErrorType.ParsingFailed,
                    SystemMessageProvider.GetErrorMessage(ConversationErrorType.ParsingFailed)
                );
            }

            if (response.candidates == null || response.candidates.Count == 0)
            {
                string blockReason = response.promptFeedback != null
                    ? response.promptFeedback.blockReason
                    : "unknown";

                ProjectLogger.Warning($"응답 후보가 없습니다. blockReason={blockReason}");

                return OperationResult<string>.Fail(
                    ConversationErrorType.EmptyLLMResponse,
                    SystemMessageProvider.GetErrorMessage(ConversationErrorType.EmptyLLMResponse)
                );
            }

            var firstCandidate = response.candidates[0];

            if (firstCandidate.content == null || firstCandidate.content.parts == null || firstCandidate.content.parts.Count == 0)
            {
                ProjectLogger.Warning("응답 content 또는 parts가 비어 있습니다.");
                return OperationResult<string>.Fail(
                    ConversationErrorType.EmptyLLMResponse,
                    SystemMessageProvider.GetErrorMessage(ConversationErrorType.EmptyLLMResponse)
                );
            }

            StringBuilder builder = new StringBuilder();

            foreach (var part in firstCandidate.content.parts)
            {
                if (!string.IsNullOrWhiteSpace(part.text))
                {
                    if (builder.Length > 0)
                        builder.Append("\n");

                    builder.Append(part.text.Trim());
                }
            }

            string finalText = builder.ToString();

            if (string.IsNullOrWhiteSpace(finalText))
            {
                ProjectLogger.Warning("parts 안에 유효한 text가 없습니다.");
                return OperationResult<string>.Fail(
                    ConversationErrorType.EmptyLLMResponse,
                    SystemMessageProvider.GetErrorMessage(ConversationErrorType.EmptyLLMResponse)
                );
            }

            return OperationResult<string>.Success(finalText);
        }
        catch (JsonException e)
        {
            ProjectLogger.Error("JSON 파싱 오류: " + e.Message);
            ProjectLogger.Error("원본 응답: " + json);

            return OperationResult<string>.Fail(
                ConversationErrorType.ParsingFailed,
                SystemMessageProvider.GetErrorMessage(ConversationErrorType.ParsingFailed)
            );
        }
        catch (System.Exception e)
        {
            ProjectLogger.Error("응답 처리 중 예외 발생: " + e.Message);

            return OperationResult<string>.Fail(
                ConversationErrorType.Unknown,
                SystemMessageProvider.GetErrorMessage(ConversationErrorType.Unknown)
            );
        }
    }

    private string EscapeJson(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";

        return text
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "");
    }
}