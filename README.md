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
- STT 오인식 보정을 위한 alias / 정규화 / Levenshtein 보정 적용
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
| Z | 단검 스킬 테스트 |
| X | 바람 스킬 테스트 |
| C | 방패 스킬 테스트 |
| V | 돌진 스킬 테스트 |

---

# 현재 스킬 목록

현재는 속성 시스템 없이, 단일 음성 명령 기반 스킬 구조로 구현되어 있습니다.

| 스킬 | 설명 |
|---|---|
| 단검 | 주변에 단검을 생성한 뒤 전방으로 발사 |
| 바람 | 전방에 지속형 바람 영역 생성 |
| 방패 | 전방에 방패형 장벽 생성 |
| 돌진 | 전방 돌진 + 짧은 이동속도 증가 |

---

# STT 명령어 보정

Whisper 로컬 모델을 `ko`로 사용하기 때문에, 스킬 명령어는 짧고 명확한 한국어 단어를 기준으로 구성했습니다.

표준 명령어는 아래와 같습니다.

```text
단검
바람
방패
돌진
```

STT 오인식에 대응하기 위해 실제 테스트에서 자주 나온 표현을 alias로 추가하고, Levenshtein Distance 기반 유사도 보정도 함께 사용합니다.

예시:

```text
단거 → 단검
바랑 → 바람
바란 → 바람
방페 → 방패
돌지 → 돌진
도진 → 돌진
```

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

---

# Whisper 모델 파일

Whisper 로컬 모델 파일(`*.bin`)은 GitHub 용량 제한으로 인해 저장소에 포함되지 않습니다.

직접 다운로드 후 아래 위치에 추가해야 합니다.

```text
Assets/StreamingAssets/
```

예시:

```text
Assets/StreamingAssets/ggml-base.bin
```

---

# 실행 방법

1. Unity Hub에서 프로젝트 열기
2. Unity 2022.3.62f3 사용 확인
3. `config.json` 생성 후 Gemini API Key 입력
4. Whisper 모델 파일 추가
5. 필요한 패키지 설치 확인
    - Newtonsoft.Json
    - Cinemachine
    - TextMeshPro
6. 메인 씬 실행

---

# 현재 개발 진행 상태

## 완료

- GitHub 협업 구조 구축
- API Key 분리 구조
- 3인칭 이동 시스템
- STT + LLM 대화 구조
- 음성 기반 스킬 구조
- 단일 명령 스킬 실행
- 실패 처리 / 테스트 UX
- 디버그 스킬 입력
- STT 명령어 인식 개선

---

## 진행 중

- 스킬 이펙트 개선
- 스킬 밸런스 조정
- 발표용 데모 흐름 정리

---

## 추가 예정

- 적 / 전투 시스템 보강
- UI 개선
- GameManager 역할 분리
- 스킬 구조 리팩토링
- 최종 polish

---

# GitHub 업로드 제외 대상

다음 파일/폴더는 GitHub에 업로드하지 않습니다.

```text
Library/
Temp/
Logs/
UserSettings/
.vscode/
Recordings/
config.json
Assets/StreamingAssets/*.bin
```

---

# 프로젝트 목표

STT와 LLM을 활용한  
실시간 음성 상호작용 게임 구조 연구 및 프로토타입 제작을 목표로 합니다.
