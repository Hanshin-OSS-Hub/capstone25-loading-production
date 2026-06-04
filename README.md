# Unity STT Skill Game

Whisper 로컬 STT를 활용해 음성 명령으로 스킬을 발동하는 Unity 기반 3D 전투 게임 데모입니다.  
플레이어는 키보드와 음성 입력을 함께 사용하여 이동, 보스 전투, 스킬 시전, UI 피드백을 확인할 수 있습니다.

---

## 1. 프로젝트 개요

본 프로젝트는 **온디바이스 STT 기반 AI 인터페이스**를 Unity 3D 전투 시스템에 적용한 게임 데모입니다.  
네트워크 연결 없이 로컬 Whisper 모델을 사용하여 한국어 음성 명령을 인식하고, 인식된 명령을 단검, 바람, 방패, 돌진 스킬로 변환하여 게임 내에서 실행합니다.

초기에는 STT 입력과 스킬 실행 구조를 독립적으로 검증하는 형태로 개발되었으며, 이후 팀원 씬과 보스 전투 시스템을 통합하여 최종 메인 씬 `MainGame` 기준으로 플레이 가능한 구조를 구성했습니다.

### 주요 기능

- R 키 기반 음성 스킬 입력
- Whisper 로컬 모델 기반 한국어 STT 처리
- STT 오인식 보정을 위한 Alias / 정규화 / Levenshtein 유사도 비교
- Z / X / C / V 키보드 스킬 테스트
- 키보드 스킬 5초 쿨타임 및 쿨타임 UI
- 단검, 바람, 방패, 돌진 4종 스킬 구현
- 보스 전투, 체력 UI, 공격 판정 연동
- 보스 조우 거리 기반 평시 / 전투 BGM 전환
- STT latency(ms) 로그 출력
- Hit Stop, Camera Shake, 결과 UI 피드백

---

## 2. 팀원 명단 및 역할

| 이름 | 역할 |
|---|---|
| 김민우 | STT 기반 스킬 입력 시스템 구현, 스킬 실행 구조 구현, 키보드 스킬 쿨타임 UI 구현, 팀원 씬 통합, BGM 전환, 성능 측정 및 문서 정리 |
| 안정빈 | 전투 시스템 통합 테스트, 실행 환경 성능 측정, 프로젝트 검증 지원 |
| 원용준 | 실행 환경 성능 측정, 프로젝트 검증 지원 |

> 팀원별 세부 역할은 최종 제출 기준에 맞춰 필요 시 보완할 수 있습니다.

---

## 3. 실행 방법 / 환경

### 개발 및 실행 환경

| 항목 | 내용 |
|---|---|
| Unity 버전 | Unity 2022.3.62f3 |
| 운영체제 | Windows 10 / 11 |
| 렌더 파이프라인 | URP |
| 입력 방식 | Keyboard + Voice |
| STT 방식 | Whisper Local Model, Korean |
| 최종 메인 씬 | `Assets/Scenes/MainGame.unity` |

### 실행 방법

1. Unity Hub에서 프로젝트 폴더를 엽니다.
2. Unity 버전이 `2022.3.62f3`인지 확인합니다.
3. Whisper 모델 파일을 아래 경로에 직접 추가합니다.

```text
Assets/StreamingAssets/
```

예시:

```text
Assets/StreamingAssets/ggml-base.bin
```

4. Unity에서 아래 씬을 엽니다.

```text
Assets/Scenes/MainGame.unity
```

5. Play 버튼을 눌러 실행합니다.
6. 이동, 카메라, 키보드 스킬, 음성 스킬, 보스 전투, UI, BGM 전환을 확인합니다.

### 조작법

| 입력 | 기능 | 비고 |
|---|---|---|
| W / A / S / D | 이동 | 플레이어 이동 |
| Shift | 달리기 | 이동 중 사용 |
| Space | 점프 | 지면 위에서 사용 |
| Mouse | 카메라 회전 | 마우스 이동 |
| R | 음성 스킬 입력 | 쿨타임 없음 |
| Z | 단검 스킬 테스트 | 키보드 쿨타임 5초 |
| X | 바람 스킬 테스트 | 키보드 쿨타임 5초 |
| C | 방패 스킬 테스트 | 키보드 쿨타임 5초 |
| V | 돌진 스킬 테스트 | 키보드 쿨타임 5초 |

---

## 4. 주요 폴더 구조

학교 공유 레포지토리 제출 형식에 맞춰, 프로젝트 구성은 아래와 같은 기준으로 정리할 수 있습니다.

```text
/docs    → 프로젝트 문서, 발표자료, 실험 결과, 다이어그램
/src     → Unity 프로젝트 소스코드 및 스크립트
/assets  → 이미지, 사운드, 데이터, 모델 등 프로젝트 리소스
```

현재 Unity 프로젝트 내부 구조는 아래와 같습니다.

```text
Assets/
├── Scenes/
│   ├── MainGame.unity
│   └── Archive/
│       ├── TEST_MapAndLogic.unity
│       ├── TEST_SttAndSkill.unity
│       └── AoF/
│
├── Scripts/
│   ├── Combat/
│   │   ├── AttackTrigger.cs
│   │   ├── EnemyHealth.cs
│   │   ├── EnemyMovement.cs
│   │   └── PlayerHealth.cs
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
│   │   ├── BgmController.cs
│   │   ├── MicrophoneRecorder.cs
│   │   ├── SttService.cs
│   │   └── WhisperTranscriber.cs
│   └── UI/
│       ├── SkillResultView.cs
│       └── SkillCooldownIconView.cs
│
├── Prefabs/
├── Materials/
├── Textures/
├── Sounds/
├── Settings/
├── StreamingAssets/
└── TextMesh Pro/
```

### 폴더 역할

| 폴더 | 설명 |
|---|---|
| `Assets/Scenes` | 최종 실행 씬과 테스트/보관 씬 |
| `Assets/Scripts/Combat` | 보스, 플레이어 체력, 공격 판정 관련 스크립트 |
| `Assets/Scripts/Gameplay` | 플레이어 이동, 카메라, 음성 입력 감지 스크립트 |
| `Assets/Scripts/Skill` | 스킬 실행, 쿨타임, 명령어 파싱, 피드백 관련 스크립트 |
| `Assets/Scripts/Systems` | STT, 마이크 녹음, BGM 등 시스템 기능 스크립트 |
| `Assets/Scripts/UI` | 스킬 결과 UI와 쿨타임 UI 스크립트 |
| `Assets/Prefabs` | 스킬, 캐릭터, 보스 관련 프리팹 |
| `Assets/Textures` | 스킬 아이콘 등 이미지 리소스 |
| `Assets/Sounds` | BGM 및 스킬 효과음 |
| `Assets/StreamingAssets` | Whisper 로컬 모델 파일 배치 위치 |

---

## 5. 스킬 목록

| 스킬 | 입력 | 설명 |
|---|---|---|
| 단검 | 음성 또는 Z | 단검 4개를 생성하여 조준 방향으로 발사. 같은 적에게는 한 번만 피해 적용 |
| 바람 | 음성 또는 X | 전방에 지속 영역을 생성하고 적을 중심으로 끌어당기며 0.3초마다 피해 적용 |
| 방패 | 음성 또는 C | 플레이어 앞에 방패를 생성하여 보스 이동을 차단 |
| 돌진 | 음성 또는 V | 플레이어가 바라보는 수평 방향으로 돌진 |

---

## 6. STT 명령어 보정

Whisper 로컬 모델을 한국어로 사용하기 때문에, 짧은 한국어 명령어를 기준으로 스킬 명령을 구성했습니다.

표준 명령어:

```text
단검
바람
방패
돌진
```

오인식 보정을 위해 Alias와 Levenshtein Distance 기반 유사도 비교를 적용했습니다.

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

## 7. 테스트 체크포인트

```text
[ ] MainGame 정상 로드
[ ] Console 빨간 에러 없음
[ ] Missing Script 없음
[ ] WASD / Shift / Space / Mouse 정상
[ ] Z/X/C/V 키보드 스킬 정상
[ ] 키보드 스킬 쿨타임 UI 정상
[ ] R 음성 스킬 정상
[ ] R 음성 스킬은 쿨타임 없이 동작
[ ] STT latency 로그 출력
[ ] 보스 추격 / 공격 / 피격 / 사망 정상
[ ] 보스 조우 BGM 전환 정상
[ ] Player 사망 후 이동 / 스킬 입력 차단 정상
```

---

## 8. GitHub 업로드 제외 대상

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

Whisper 모델 파일은 용량 문제로 GitHub에 포함하지 않으며, 실행 환경에서 직접 `Assets/StreamingAssets/`에 추가해야 합니다.

---

## 9. 프로젝트 목표

본 프로젝트의 목표는 네트워크 연결 없이 로컬 환경에서 동작하는 STT 기반 음성 인터페이스를 Unity 3D 전투 시스템에 적용하고, 음성 명령과 키보드 입력을 함께 활용할 수 있는 전투 게임 데모를 구현하는 것입니다. 이를 통해 온디바이스 AI 인터페이스가 게임 입력 방식으로 활용될 수 있는 가능성을 확인하고자 했습니다.
