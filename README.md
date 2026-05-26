# Unity STT Skill Game

Whisper 로컬 STT를 활용해 음성 명령으로 스킬을 발동하는 Unity 기반 3D 전투 게임 데모입니다.

초기에는 STT 기반 스킬 입력과 스킬 실행 구조를 독립적으로 검증하는 분할 프로젝트였고, 현재는 팀원이 제공한 씬과 전투 시스템을 통합하여 `MainGame` 기준으로 플레이어 이동, 보스 전투, 음성 스킬, 키보드 스킬 테스트, UI 피드백, BGM 전환까지 함께 동작하는 상태입니다.

---

# 개발 환경

- Unity: 2022.3.62f3
- OS: Windows
- STT: Whisper Local Model (ko)
- Input: Keyboard + Voice
- Camera: Custom CameraMovement
- Render Pipeline: URP
- 최종 메인 씬: `Assets/Scenes/MainGame.unity`

---

# 주요 기능

## STT 기반 스킬 입력

- R 키 기반 음성 스킬 입력
- Whisper 로컬 모델 기반 한국어 음성 인식
- 짧은 입력 취소 처리
- STT 실패 / 알 수 없는 명령 처리
- STT 오인식 보정을 위한 alias / 정규화 / Levenshtein 유사도 비교 적용
- R 키 입력부터 스킬 시전까지의 latency(ms) 로그 출력
- 플레이어 사망 시 R 입력 및 스킬 실행 차단

## 키보드 기반 스킬 테스트

- Z / X / C / V 키로 스킬 즉시 테스트 가능
- 키보드 스킬은 스킬별 5초 쿨타임 적용
- 스킬 아이콘 위에 쿨타임 오버레이 및 남은 시간 표시
- 음성 명령 스킬은 키보드 쿨타임과 별도로 동작하며, 쿨타임 없이 사용 가능

## 스킬 실행 시스템

- 단검, 바람, 방패, 돌진 4종 스킬 구현
- 카메라 조준 방향 기반 스킬 발동
- 단검은 4개가 발사되며, 같은 적에게 여러 개가 맞아도 1회만 피해 적용
- 바람은 전방에 생성되는 지속형 구체 영역이며, 적을 중심으로 끌어당기고 주기적으로 피해 적용
- 방패는 적 이동을 막는 장애물로 동작
- 돌진은 플레이어가 바라보는 수평 방향으로 이동하며 꼬리 / 이펙트 표시
- Hit Stop 및 Camera Shake 피드백 적용
- 결과 UI를 통한 스킬 인식 / 발동 / 실패 표시

## 팀원 씬 / 전투 시스템 통합

- 팀원이 제공한 `AoF` 씬을 기반으로 최종 메인 씬 `MainGame` 구성
- 팀원 보스 몬스터 / 플레이어 체력 UI / 공격 판정 시스템 연동
- 스킬 피해를 `EnemyHealth`와 연결
- 보스 몬스터가 공격 시 제자리에서 멈춘 뒤 공격하도록 보정
- 보스 몬스터 사망 시 공격 모션을 끊고 즉시 사망 모션으로 전환
- 보스 몬스터 크기 2배 조정
- 보스 조우 시 전투 BGM 재생, 멀어지면 평시 BGM 복귀

## 플레이어 / 카메라 시스템

- WASD 이동
- Shift 달리기
- Space 점프
- 마우스 카메라 회전
- 직접 구현한 `CameraMovement` 기반 카메라 제어
- Cinemachine 의존성 제거
- 플레이어 사망 시 이동 및 스킬 입력 차단

## UI 시스템

- 좌측 하단 스킬 단축키 안내 UI
- 스킬 아이콘 UI
- 키보드 스킬 쿨타임 오버레이 / 숫자 표시
- Player HP UI: 우측 하단 앵커
- Enemy HP UI: 상단 중앙 앵커
- 스킬 결과 UI: 인식 결과, 성공, 실패 메시지 표시

---

# 조작법

| 입력 | 기능 | 비고 |
|---|---|---|
| W / A / S / D | 이동 | 플레이어 이동 |
| Shift | 달리기 | 이동 중 사용 |
| Space | 점프 | 지면 위에서 사용 |
| Mouse | 카메라 회전 | 마우스 이동 |
| R | 스킬 음성 입력 | 쿨타임 없음 |
| Z | 단검 스킬 테스트 | 키보드 쿨타임 5초 |
| X | 바람 스킬 테스트 | 키보드 쿨타임 5초 |
| C | 방패 스킬 테스트 | 키보드 쿨타임 5초 |
| V | 돌진 스킬 테스트 | 키보드 쿨타임 5초 |

---

# 현재 스킬 목록

| 내부 ID | 한글명 | 설명 |
|---|---|---|
| Blade | 단검 | 좌상 / 우상 / 좌하 / 우하 위치에서 단검 4개를 생성하고 조준 방향으로 발사. 같은 적에게는 한 번만 피해 적용 |
| Storm | 바람 | 조준 방향 앞쪽에 원형 구체 형태의 지속 영역 생성. 적을 중심으로 끌어당기고 0.3초마다 1 피해 적용 |
| Barrier | 방패 | 플레이어 앞 지면 기준 방향에 방패 생성. 보스 몬스터가 통과하지 못하도록 차단 |
| Dash | 돌진 | 플레이어가 바라보는 수평 방향으로 돌진. 꼬리 / 이펙트 표시, 충돌 피해 없음 |

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
│   ├── Blade.prefab
│   ├── Storm.prefab
│   ├── Barrier.prefab
│   ├── Meshy_AI_Elderwood_Sentinel_biped/
│   └── Meshy_AI_Rugged_Drifter_biped/
├── Scenes/
│   ├── MainGame.unity
│   └── Archive/
│       ├── TEST_MapAndLogic.unity
│       ├── TEST_SttAndSkill.unity
│       └── AoF/
├── Scripts/
│   ├── Combat/
│   │   ├── AttackTrigger.cs
│   │   ├── EnemyHealth.cs
│   │   ├── EnemyMovement.cs
│   │   └── PlayerHealth.cs
│   ├── Debug/
│   │   └── ProjectLogger.cs
│   ├── Gameplay/
│   │   ├── CameraMovement.cs
│   │   ├── PlayerMovement.cs
│   │   └── PushToSkillInput.cs
│   ├── Skill/
│   │   ├── Core/
│   │   │   ├── SkillCooldownManager.cs
│   │   │   ├── SkillCoordinator.cs
│   │   │   ├── SkillExecutor.cs
│   │   │   └── SkillId.cs
│   │   ├── Debug/
│   │   │   └── SkillDebugInput.cs
│   │   ├── Execution/
│   │   │   ├── BladeSkillProjectile.cs
│   │   │   ├── StormSkillZone.cs
│   │   │   ├── BarrierSkillObject.cs
│   │   │   └── DashSkillRunner.cs
│   │   ├── Feedback/
│   │   └── Parsing/
│   ├── Systems/
│   │   ├── AudioRecordingData.cs
│   │   ├── BgmController.cs
│   │   ├── MicrophoneRecorder.cs
│   │   ├── OperationResult.cs
│   │   ├── Recorder.cs
│   │   ├── SttService.cs
│   │   ├── SystemErrorType.cs
│   │   ├── SystemMessageProvider.cs
│   │   └── WhisperTranscriber.cs
│   └── UI/
│       ├── SkillResultView.cs
│       └── SkillCooldownIconView.cs
├── Settings/
├── Sounds/
│   ├── mainBGM.mp3
│   ├── Wierd View.mp3
│   ├── Wild Hunt.mp3
│   ├── blade.mp3
│   ├── barrier.wav
│   ├── dash.wav
│   └── wind.wav
├── Textures/
│   ├── blade_UI.png
│   ├── storm_UI.png
│   ├── barrier_UI.png
│   └── dash_UI.png
├── StreamingAssets/
└── TextMesh Pro/
```

`Scenes/Archive` 폴더는 이전 테스트 씬과 팀원 원본 씬을 보관하는 영역입니다. 최종 시연 및 실행은 `Assets/Scenes/MainGame.unity`를 기준으로 진행합니다.

---

# 씬 구조

현재 최종 메인 씬은 아래 파일입니다.

```text
Assets/Scenes/MainGame.unity
```

권장 Hierarchy 구조는 다음과 같습니다.

```text
MainGame
├── Main Camera
├── Directional Light
├── EventSystem
├── Player
├── Boss / Enemy
├── GameManager
│   ├── InputManager
│   ├── STTManager
│   ├── SkillManager
│   ├── FeedbackManager
│   └── AudioManager
└── SkillHUDCanvas
    └── SkillGuideGroup
```

각 Manager의 역할:

| 오브젝트 | 역할 |
|---|---|
| InputManager | R 키 음성 입력, Z/X/C/V 키보드 스킬 입력, 키보드 스킬 쿨타임 관리 |
| STTManager | 마이크 녹음, Whisper 전사, STT 서비스 |
| SkillManager | STT 결과 파싱, 스킬 실행 흐름 제어, STT latency 로그 출력 |
| FeedbackManager | Camera Shake, Hit Stop |
| AudioManager | 평시 / 전투 BGM 전환 |
| SkillHUDCanvas | 스킬 결과 UI, 스킬 단축키 안내, 쿨타임 아이콘 UI |

---

# 주요 스크립트 역할

| 스크립트 | 역할 |
|---|---|
| SkillCoordinator | R 음성 입력 흐름 제어, STT 처리, 명령 파싱, 스킬 실행, latency 측정 |
| SkillExecutor | 스킬별 실행 로직 담당 |
| SkillCooldownManager | Z/X/C/V 키보드 스킬 쿨타임 관리 |
| SkillCooldownIconView | 스킬 아이콘 쿨타임 오버레이 / 숫자 표시 |
| SkillDebugInput | 키보드 스킬 테스트 입력 처리 |
| BgmController | 보스 조우 거리 기반 BGM 전환 |
| EnemyHealth | 보스 체력 및 피격 처리 |
| EnemyMovement | 보스 추격, 공격, 사망 처리 |
| PlayerHealth | 플레이어 체력 및 사망 처리 |
| Recorder | 스크린캡쳐용 스크립트 |

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
5. `Assets/Scenes/MainGame.unity` 열기
6. Play 실행
7. WASD / Mouse로 이동과 카메라 확인
8. Z/X/C/V 키보드 입력으로 스킬과 쿨타임 UI 테스트
9. R 음성 입력으로 STT 스킬 테스트
10. 보스 조우, 전투 BGM, 보스 피격 / 사망 동작 확인

---

# 테스트 체크포인트

```text
[ ] MainGame 정상 로드
[ ] Console 빨간 에러 없음
[ ] Missing Script 없음
[ ] WASD / Shift / Space / Mouse 정상
[ ] Enemy 추격 / 공격 / 사망 정상
[ ] Enemy 공격 중 사망 시 즉시 사망 모션 전환
[ ] Z/X/C/V 키보드 스킬 정상
[ ] 키보드 스킬 5초 쿨타임 정상
[ ] 스킬 아이콘 쿨타임 UI 정상
[ ] R 음성 스킬 정상
[ ] R 음성 스킬은 쿨타임 없이 동작
[ ] STT latency 로그 출력
[ ] 보스 조우 BGM 전환 정상
[ ] Player 사망 후 이동 / 스킬 입력 차단
```

---

# 현재 개발 진행 상태

## 완료

- GitHub 관리 구조 구축
- API Key 의존성 제거
- LLM / 무닌 / 대화 시스템 제거
- GameManager 역할 분리
- 3인칭 이동 및 카메라 구조 안정화
- 팀원 제공 씬 / 에셋 / 전투 시스템 통합
- 최종 메인 씬 `MainGame` 구성
- 테스트 / 원본 씬을 `Scenes/Archive`로 정리
- STT 기반 스킬 입력 구조 구현
- 키보드 디버그 스킬 입력 구현
- 키보드 스킬 5초 쿨타임 구현
- 스킬 아이콘 및 쿨타임 UI 구현
- 단검 / 바람 / 방패 / 돌진 스킬 구현
- 스킬 피해를 `EnemyHealth`와 연결
- 보스 공격 / 사망 동작 보정
- 보스 조우 BGM 전환 구현
- STT latency(ms) 로그 구현
- UI 앵커 정리
- Hit Stop / Camera Shake 적용
- 스킬 결과 UI 구현
- STT 명령어 alias / 유사도 보정
- 불필요 코드 및 말풍선 관련 코드 제거
- SkillExecutor 리팩토링
- 통합 기능 테스트 진행
- 통합 완료 이후 폴더 구조 정리

## 다음 예정

- main 기준 최종 테스트
- 발표 / 제출용 최종 정리

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

STT 기반 음성 명령으로 실시간 스킬을 발동하는 구조를 검증하고, 팀원 전투 씬과 통합하여 음성 명령 / 키보드 테스트 / 보스 전투 / UI 피드백 / BGM 전환까지 포함한 Unity 3D 전투 게임 데모를 제작하는 것을 목표로 합니다.
