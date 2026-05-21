# Unity STT Skill Game Prototype

Whisper 로컬 STT를 활용해 음성 명령으로 스킬을 발동하는 Unity 기반 3D 스킬 프로토타입입니다.

이 프로젝트는 전체 팀 프로젝트에 병합하기 전, STT 기반 스킬 입력과 스킬 실행 구조를 독립적으로 검증하기 위한 분할 프로젝트입니다.

---

# 개발 환경

- Unity: 2022.3.62f3
- OS: Windows
- STT: Whisper Local Model (ko)
- Input: Keyboard + Voice
- Camera: Custom CameraMovement
- Render Pipeline: URP

---

# 주요 기능

## STT 기반 스킬 입력

- R 키 기반 음성 스킬 입력
- Whisper 로컬 모델 기반 한국어 음성 인식
- 짧은 입력 취소 처리
- STT 실패 / 알 수 없는 명령 처리
- STT 오인식 보정을 위한 alias / 정규화 / Levenshtein 유사도 비교 적용

## 키보드 기반 스킬 테스트

- Z / X / C / V 키로 스킬 즉시 테스트 가능
- STT 없이 스킬 실행 결과를 빠르게 확인할 수 있음

## 스킬 실행 시스템

- 단검, 바람, 방패, 돌진 4종 스킬 구현
- 스킬별 프리팹 생성 및 실행
- 카메라 조준 방향 기반 스킬 발동
- 방패 / 돌진은 지면 수평 방향 기준으로 안정화
- Hit Stop 및 Camera Shake 피드백 적용
- 결과 UI를 통한 스킬 인식 / 발동 / 실패 표시

## 플레이어 / 카메라 시스템

- WASD 이동
- Shift 달리기
- Space 점프
- 마우스 카메라 회전
- 직접 구현한 CameraMovement 기반 카메라 제어
- Cinemachine 의존성 제거

---

# 조작법

| 입력 | 기능 |
|---|---|
| W / A / S / D | 이동 |
| Shift | 달리기 |
| Space | 점프 |
| Mouse | 카메라 회전 |
| R | 스킬 음성 입력 |
| Z | 단검 스킬 테스트 |
| X | 바람 스킬 테스트 |
| C | 방패 스킬 테스트 |
| V | 돌진 스킬 테스트 |

---

# 현재 스킬 목록

| 내부 ID | 한글명 | 설명 |
|---|---|---|
| Blade | 단검 | 좌상 / 우상 / 좌하 / 우하 위치에서 단검 4개를 생성하고 조준 방향으로 발사 |
| Storm | 바람 | 조준 방향 앞쪽에 지속형 바람 영역 생성 |
| Barrier | 방패 | 플레이어 앞 지면 기준 방향에 방패 생성 |
| Dash | 돌진 | 플레이어가 바라보는 수평 방향으로 돌진하고 짧은 이동속도 증가 적용 |

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
단건 → 단검
당검 → 단검

바란 → 바람
바랑 → 바람
파람 → 바람

방페 → 방패
반패 → 방패

돌지 → 돌진
도진 → 돌진
```

---

# 프로젝트 구조

```text
Assets/
├── Materials/
├── Prefabs/
├── Scenes/
├── Scripts/
│   ├── Debug/
│   │   └── ProjectLogger.cs
│   ├── Gameplay/
│   │   ├── CameraMovement.cs
│   │   ├── PlayerMovement.cs
│   │   └── PushToSkillInput.cs
│   ├── Skill/
│   │   ├── Core/
│   │   ├── Debug/
│   │   ├── Execution/
│   │   ├── Feedback/
│   │   └── Parsing/
│   ├── Systems/
│   │   ├── AudioRecordingData.cs
│   │   ├── MicrophoneRecorder.cs
│   │   ├── OperationResult.cs
│   │   ├── SttService.cs
│   │   ├── SystemErrorType.cs
│   │   ├── SystemMessageProvider.cs
│   │   └── WhisperTranscriber.cs
│   └── UI/
│       └── SkillResultView.cs
├── StreamingAssets/
└── TextMesh Pro/
```

---

# 씬 구조

메인 씬은 `MainTechnologyScene`입니다.

권장 Hierarchy 구조는 다음과 같습니다.

```text
MainTechnologyScene
├── Main Camera
├── Directional Light
├── EventSystem
├── Plane
├── Pinguin_001
├── Chicken_001
├── GameManager
│   ├── InputManager
│   ├── STTManager
│   ├── SkillManager
│   └── FeedbackManager
└── SkillHUDCanvas
```

각 Manager의 역할:

| 오브젝트 | 역할 |
|---|---|
| InputManager | R 키 음성 입력, Z/X/C/V 디버그 스킬 입력 |
| STTManager | 마이크 녹음, Whisper 전사, STT 서비스 |
| SkillManager | STT 결과 파싱, 스킬 실행 흐름 제어 |
| FeedbackManager | Camera Shake, Hit Stop |
| SkillHUDCanvas | 스킬 결과 UI 표시 |

---

# Whisper 모델 파일

Whisper 로컬 모델 파일(`*.bin`)은 GitHub 용량 제한으로 인해 저장소에 포함하지 않습니다.

직접 다운로드 후 아래 위치에 추가해야 합니다.

```text
Assets/StreamingAssets/
```

예시:

```text
Assets/StreamingAssets/ggml-base.bin
```

Whisper는 로컬 모델을 사용하므로 별도의 API Key가 필요하지 않습니다.

---

# 실행 방법

1. Unity Hub에서 프로젝트 열기
2. Unity 2022.3.62f3 사용 확인
3. Whisper 모델 파일을 `Assets/StreamingAssets/`에 추가
4. 필요한 패키지 설치 확인
    - TextMeshPro
    - Whisper Unity 관련 패키지
    - URP
5. `Assets/Scenes/MainTechnologyScene.unity` 열기
6. Play 실행
7. Z/X/C/V 또는 R 음성 입력으로 스킬 테스트

---

# 현재 개발 진행 상태

## 완료

- GitHub 관리 구조 구축
- API Key 의존성 제거
- LLM / 무닌 / 대화 시스템 제거
- GameManager 역할 분리
- 3인칭 이동 및 카메라 구조 안정화
- STT 기반 스킬 입력 구조 구현
- 키보드 디버그 스킬 입력 구현
- 단검 / 바람 / 방패 / 돌진 스킬 구현
- Hit Stop / Camera Shake 적용
- 스킬 결과 UI 구현
- STT 명령어 alias / 유사도 보정
- 불필요 코드 및 말풍선 관련 코드 제거
- SkillExecutor 리팩토링
- 최종 기능 테스트 완료

## 다음 예정

- 병합 준비 체크리스트 작성
- 팀 프로젝트와 병합
- 병합 이후 Dummy Enemy 구현
- 스킬 데미지 / 피격 판정 연결
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
Assets/StreamingAssets/*.bin
```

---

# 프로젝트 목표

STT 기반 음성 명령으로 실시간 스킬을 발동하는 구조를 검증하고, 이후 팀 프로젝트에 병합 가능한 독립 모듈 형태의 Unity 프로토타입을 제작하는 것을 목표로 합니다.
