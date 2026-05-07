# 3D Acoustic AI Interaction Project

Whisper STT와 Gemini LLM을 활용한 Unity 기반 3D 음성 상호작용 프로젝트입니다.

플레이어는 음성을 통해 NPC와 대화할 수 있으며,
음성 명령을 사용해 스킬을 발동할 수 있습니다.

---

# 개발 환경

- Unity: 2022.3.62f3
- OS: Windows
- STT: Whisper Local Model (ko)
- LLM: Gemini
- Input: Keyboard + Voice
- Camera: Cinemachine Third Person

---

# 주요 기능

## 대화 시스템
- T 키 기반 Push-To-Talk 구조
- Whisper 기반 음성 인식
- Gemini 기반 NPC 응답 생성
- 무닌(Muninn) 페르소나 적용
- 말풍선 UI 출력

---

## 스킬 시스템
- R 키 기반 음성 스킬 입력
- STT 결과 기반 스킬 판정
- 단일 명령 스킬 구조 구현
- 디버그 키 기반 테스트 가능

---

## 3인칭 캐릭터 시스템
- Third Person Movement
- Cinemachine 카메라
- WASD 이동
- 캐릭터 회전 처리

---

# 조작법

| 입력 | 기능 |
|---|---|
| T | 대화 음성 입력 |
| R | 스킬 음성 입력 |
| Z | 칼날 스킬 테스트 |
| X | 회오리 스킬 테스트 |
| C | 벽 스킬 테스트 |
| V | 돌진 스킬 테스트 |
| Q | 속성 선택 UI (추가 예정) |

---

# 현재 스킬 목록

현재는 바람 속성 기준으로 테스트 중입니다.

| 스킬 | 설명 |
|---|---|
| 칼날 | 주변에 칼날 생성 후 전방 발사 |
| 회오리 | 전방에 지속형 회오리 생성 |
| 벽 | 전방에 장벽 생성 |
| 돌진 | 전방 돌진 + 이동속도 증가 |

---

# 프로젝트 구조

```text
Assets/
├── Fonts/
├── Prefabs/
├── Prompts/
├── Scenes/
├── Scripts/
│   ├── AI/
│   ├── Audio/
│   ├── Character/
│   ├── Conversation/
│   ├── Core/
│   ├── Debug/
│   ├── Input/
│   ├── Skill/
│   │   ├── Core/
│   │   ├── Execution/
│   │   ├── Parsing/
│   │   └── Debug/
│   ├── STT/
│   ├── UI/
│   └── Utils/
├── StreamingAssets/
└── TextMesh Pro/
```

---

# API Key 설정

이 프로젝트는 API Key를 코드에 직접 저장하지 않습니다.

## 설정 방법

1. `config.example.json` 파일을 복사합니다.
2. 파일 이름을 `config.json`으로 변경합니다.
3. Gemini API Key를 입력합니다.

예시:

```json
{
  "GEMINI_API_KEY": "YOUR_GEMINI_API_KEY"
}
```

---

## 주의

`config.json`은 `.gitignore`에 포함되어 GitHub에 업로드되지 않습니다.

Whisper는 로컬 모델을 사용하므로 별도의 API Key가 필요하지 않습니다.
