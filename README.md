# capstone25-loading-production

온디바이스 음성 인식 파이프라인을 Unity 3D 전투 게임 환경에 적용한 캡스톤디자인 프로젝트입니다.  
네트워크 연결 없이 로컬 Whisper STT 모델을 사용하여 한국어 음성 명령을 인식하고, 인식된 명령을 게임 내 스킬 입력으로 변환합니다.

---

## 프로젝트 개요

본 프로젝트는 기존 키보드·마우스 중심의 물리 입력 방식을 보완하기 위해, **음성 명령 기반 멀티모달 인터페이스**를 Unity 기반 3D 디지털 콘텐츠에 통합하는 것을 목표로 합니다.

사용자는 키보드와 마우스로 기본 이동 및 카메라 조작을 수행하고, R 키를 이용한 음성 입력 또는 Z / X / C / V 키보드 입력을 통해 스킬을 발동할 수 있습니다. 음성 명령은 로컬 Whisper STT 모델을 통해 텍스트로 변환되며, Alias 매핑과 Levenshtein Distance 기반 유사도 보정을 거쳐 게임 내 스킬 실행으로 연결됩니다.

최종 구현물은 `MainGame` 씬을 기준으로 플레이어 이동, 보스 전투, 음성 스킬, 키보드 스킬 테스트, UI 피드백, BGM 전환이 함께 동작하는 Unity 3D 전투 게임 데모입니다.

### 주요 기능

- R 키 기반 음성 스킬 입력
- 로컬 Whisper 모델 기반 한국어 STT 처리
- 네트워크/API Key 의존성 없는 온디바이스 실행 구조
- Alias / 정규화 / Levenshtein Distance 기반 STT 오인식 보정
- Z / X / C / V 키보드 스킬 테스트
- 키보드 스킬 5초 쿨타임 및 쿨타임 UI
- 단검, 바람, 방패, 돌진 4종 스킬 구현
- 보스 전투, 체력 UI, 공격 판정 연동
- 보스 조우 거리 기반 평시 / 전투 BGM 전환
- STT latency(ms) 로그 출력
- Hit Stop, Camera Shake, 결과 UI 피드백

---

## 팀원 명단 및 역할

| 이름 | 역할 |
|---|---|
| 안정빈 | 팀장, 오디오/BGM 제작 및 믹싱, AI 기반 3D 에셋 제작, 전투 BGM 전환 설정, 발표/보고서 문서 보완 |
| 원용준 | 3D 레벨 디자인 및 맵 환경 구축, 에셋 라이브러리 관리, 물리 충돌 로직 최적화, 논문/결과보고서 양식 및 오탈자 검수 |
| 김민우 | STT 기반 스킬 입력 시스템 구현, Whisper 로컬 STT 연동, 스킬 실행 및 VFX 구현, Alias/Levenshtein 보정, latency 로그 및 정량 측정 정리 |

---

## 실행 방법 / 환경

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

## 주요 폴더 구조 예시

학교 공유 레포지토리 제출 형식에 맞춘 상위 구조 예시는 다음과 같습니다.

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

## 스킬 목록

| 스킬 | 음성 명령 | 키보드 테스트 | 설명 |
|---|---|---|---|
| 단검 | 단검 | Z | 단검 4개를 생성하여 조준 방향으로 발사. 같은 적에게는 한 번만 피해 적용 |
| 바람 | 바람 | X | 전방에 지속 영역을 생성하고 적을 중심으로 끌어당기며 0.3초마다 피해 적용 |
| 방패 | 방패 | C | 플레이어 앞에 방패를 생성하여 보스 이동을 차단 |
| 돌진 | 돌진 | V | 플레이어가 바라보는 수평 방향으로 돌진 |

---

## STT 명령어 보정

Whisper 로컬 모델을 한국어로 사용하기 때문에, 짧고 명확한 한국어 명령어를 기준으로 스킬 명령을 구성했습니다.

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

## 정량 평가 요약

서로 다른 4개 PC 환경에서 동일한 음성 입력 과정을 각각 10회 반복 측정했습니다. 측정 구간은 R 키를 누른 시점부터 스킬이 시전되기까지의 전체 체감 지연 시간과, R 키를 뗀 시점부터 스킬이 시전되기까지의 시스템 처리 지연 시간으로 구분했습니다.

| PC 환경 | R 키다운 → 스킬 시전 ms | R 릴리즈 → 스킬 시전 ms |
|---|---:|---:|
| Desktop A | 2315.1 ± 108.9 | 980.9 ± 12.1 |
| Laptop A | 3734.9 ± 412.2 | 1682.8 ± 78.8 |
| Laptop B | 4600.2 ± 215.5 | 2536.6 ± 93.9 |
| Laptop C | 8038.7 ± 474.1 | 6017.8 ± 711.8 |

측정 결과, 고성능 PC 환경인 Desktop A에서 가장 낮은 평균 지연 시간이 나타났고, 저전력 기반 환경인 Laptop C에서 가장 높은 지연 시간이 나타났습니다. 이를 통해 로컬 Whisper STT 처리 시간은 하드웨어 성능과 실행 환경의 영향을 크게 받을 수 있음을 확인했습니다.

---

## 테스트 체크포인트

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

## GitHub 업로드 제외 대상

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

## 참고 문서

- 졸업논문: 온디바이스 음성 인식 파이프라인 기반 디지털 콘텐츠 인터페이스의 확장성 연구
- 최종 결과보고서: 음성 인식을 활용한 게임 제작
- 발표 자료: AI·SW 캡스톤디자인 2 최종 발표 PPT
- 연구일지 #13: 프로젝트 최종 마무리 및 문서 작업과 정리
