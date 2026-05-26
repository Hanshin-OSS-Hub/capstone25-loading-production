# 병합 준비 체크리스트

이 문서는 `stt-skill-game` 프로젝트의 `merge-teammate-project` 브랜치에서 진행한 팀원 씬 통합 결과를 `main`에 병합하기 전에 확인해야 할 항목을 정리한 문서입니다.

현재 프로젝트의 핵심 범위는 다음과 같습니다.

```text
팀원 제공 AoF 씬 / 전투 시스템
+ Whisper Local STT
+ 음성 명령 스킬 입력
+ 키보드 스킬 테스트
+ 스킬 전투 연동
+ 쿨타임 UI
+ 보스 조우 BGM
+ STT latency 측정
```

LLM, Gemini, 무닌, 대화 시스템, 말풍선 UI는 현재 범위에서 제거되었습니다.

---

# 1. 병합 전 필수 확인

## 1-1. Unity 버전

현재 프로젝트 기준:

```text
Unity 2022.3.62f3
```

팀 프로젝트와 Unity 버전이 다르면 아래 문제가 생길 수 있습니다.

```text
패키지 버전 차이
씬/프리팹 직렬화 변경
ProjectSettings 자동 변경
ShaderGraph / URP 설정 변경
```

병합 전 확인:

```text
[ ] Unity 2022.3.62f3 또는 같은 2022.3 LTS 계열 사용
[ ] 병합 전 별도 브랜치에서 작업
[ ] Unity Console 빨간 에러 없음
[ ] Missing Script 없음
[ ] Git working tree clean 상태 확인
```

---

# 2. 최종 사용 씬

현재 최종 통합 씬은 아래 파일입니다.

```text
Assets/Scenes/FinMergeScene.unity
```

기존 `MainTechnologyScene`은 STT / 스킬 단독 검증용에 가깝고, 현재 통합 결과 확인은 `FinMergeScene` 기준으로 진행합니다.

체크:

```text
[ ] FinMergeScene 정상 로드
[ ] Play 실행 가능
[ ] Console 빨간 에러 없음
[ ] Missing Script 없음
[ ] 팀원 보스 / 플레이어 / UI / BGM / STT 스킬 정상 동작
```

---

# 3. 병합해야 할 주요 폴더 / 파일

## 3-1. Scripts

아래 폴더는 현재 기능의 핵심입니다.

```text
Assets/Scripts/Debug/
Assets/Scripts/Gameplay/
Assets/Scripts/Skill/
Assets/Scripts/Systems/
Assets/Scripts/UI/
Assets/TeamMerge/Teammate/Scripts/
```

핵심 스크립트:

```text
Debug/ProjectLogger.cs

Gameplay/CameraMovement.cs
Gameplay/PlayerMovement.cs
Gameplay/PushToSkillInput.cs

Systems/AudioRecordingData.cs
Systems/BgmController.cs
Systems/MicrophoneRecorder.cs
Systems/OperationResult.cs
Systems/SttService.cs
Systems/SystemErrorType.cs
Systems/SystemMessageProvider.cs
Systems/WhisperTranscriber.cs

Skill/Core/SkillCastResult.cs
Skill/Core/SkillCooldownManager.cs
Skill/Core/SkillCoordinator.cs
Skill/Core/SkillExecutor.cs
Skill/Core/SkillId.cs
Skill/Core/SkillMessages.cs
Skill/Core/SkillNameProvider.cs

Skill/Debug/SkillDebugInput.cs
Skill/Execution/BarrierSkillObject.cs
Skill/Execution/BladeSkillProjectile.cs
Skill/Execution/DashSkillRunner.cs
Skill/Execution/StormSkillZone.cs
Skill/Feedback/HitStopController.cs
Skill/Feedback/SkillCameraShake.cs
Skill/Parsing/SkillCommandParser.cs

UI/SkillResultView.cs
UI/SkillCooldownIconView.cs

TeamMerge/Teammate/Scripts/EnemyHealth.cs
TeamMerge/Teammate/Scripts/EnemyMovement.cs
TeamMerge/Teammate/Scripts/PlayerHealth.cs
TeamMerge/Teammate/Scripts/AttackTrigger.cs
TeamMerge/Teammate/Scripts/Recorder.cs
```

체크:

```text
[ ] 위 스크립트 폴더 전체 포함
[ ] Unity에서 .meta 파일과 함께 관리
[ ] 파일명과 클래스명 일치 확인
[ ] Console 컴파일 에러 없음 확인
```

---

## 3-2. Prefabs

스킬 실행에 필요한 프리팹:

```text
Assets/Prefabs/Blade.prefab
Assets/Prefabs/Storm.prefab
Assets/Prefabs/Barrier.prefab
```

팀원 캐릭터 / 보스 프리팹:

```text
Assets/TeamMerge/Teammate/Prefabs/
```

체크:

```text
[ ] Blade 프리팹 포함
[ ] Storm 프리팹 포함
[ ] Barrier 프리팹 포함
[ ] 팀원 Player / Boss 관련 프리팹 포함
[ ] 각 프리팹의 Material / Texture / VFX 참조 누락 없음
[ ] Missing Script 없음
[ ] 방패 프리팹의 Enemy 차단 동작 정상
```

---

## 3-3. Materials / VFX / Textures

스킬 프리팹과 팀원 씬이 참조하는 Material, Texture, VFX도 함께 필요합니다.

현재 사용되는 주요 에셋:

```text
Assets/Materials/
Assets/Eric VFX Studio/
Assets/Pure Poly/
Assets/Textures/
Assets/TeamMerge/Teammate/Prefabs/
```

스킬 UI 이미지:

```text
Assets/Textures/blade_UI.png
Assets/Textures/storm_UI.png
Assets/Textures/barrier_UI.png
Assets/Textures/dash_UI.png
```

체크:

```text
[ ] 스킬 프리팹에 연결된 Material 포함
[ ] 팀원 씬 Material / Texture 포함
[ ] 스킬 UI 이미지 포함
[ ] VFX 관련 Material / Animator / Texture 포함
[ ] 병합 후 프리팹이 분홍색으로 깨지지 않는지 확인
```

---

## 3-4. UI

현재 UI는 다음 기능을 포함합니다.

```text
SkillResultView
SkillGuideGroup
SkillCooldownIconView
Player_HP_Slider
Enemy_HP_Slider
```

체크:

```text
[ ] SkillHUDCanvas 존재
[ ] SkillResultView 연결
[ ] SkillGuideGroup 왼쪽 하단 배치
[ ] 스킬 아이콘 4개 표시
[ ] 쿨타임 오버레이 / 숫자 표시
[ ] Player_HP_Slider 오른쪽 하단 앵커
[ ] Enemy_HP_Slider 상단 중앙 앵커
[ ] UI가 해상도 변경 시 크게 틀어지지 않음
```

---

## 3-5. Sounds / BGM

보스 조우 BGM 전환에 필요한 파일:

```text
Assets/TeamMerge/Teammate/Sounds/mainBGM.mp3
Assets/TeamMerge/Teammate/Sounds/Wierd View.mp3
Assets/TeamMerge/Teammate/Sounds/Wild Hunt.mp3
Assets/TeamMerge/Teammate/Resources/Sounds/kaak.mp3
```

체크:

```text
[ ] AudioManager 존재
[ ] BgmController 연결
[ ] 평시 BGM 연결
[ ] 전투 BGM 연결
[ ] 보스 조우 시 전투 BGM 전환
[ ] 보스와 멀어지면 평시 BGM 복귀
```

---

## 3-6. Whisper 모델 파일

Whisper 로컬 모델 파일은 GitHub에 포함되지 않습니다.

필요 위치:

```text
Assets/StreamingAssets/
```

예시:

```text
Assets/StreamingAssets/ggml-base.bin
```

체크:

```text
[ ] StreamingAssets 폴더 존재
[ ] Whisper 모델 .bin 파일 직접 추가
[ ] .gitignore에서 *.bin 제외 유지
[ ] STT 실행 시 모델 로딩 오류 없음 확인
```

---

# 4. FinMergeScene 권장 Hierarchy 구조

```text
FinMergeScene
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

# 5. Manager별 컴포넌트 배치

## 5-1. InputManager

```text
PushToSkillInput
SkillDebugInput
SkillCooldownManager
```

필수 연결:

```text
SkillDebugInput → SkillExecutor
SkillDebugInput → SkillCooldownManager
SkillDebugInput → SkillCameraShake
SkillDebugInput → HitStopController
SkillDebugInput → SkillResultView
```

정책:

```text
Z/X/C/V 키보드 스킬 → 5초 쿨타임 적용
R 음성 스킬 → 쿨타임 없이 동작
```

---

## 5-2. STTManager

```text
MicrophoneRecorder
WhisperManager
WhisperTranscriber
SttService
```

필수 연결:

```text
WhisperTranscriber → WhisperManager
SttService → MicrophoneRecorder
SttService → WhisperTranscriber
```

---

## 5-3. SkillManager

```text
SkillCommandParser
SkillCoordinator
```

필수 연결:

```text
SkillCoordinator → PushToSkillInput
SkillCoordinator → SttService
SkillCoordinator → SkillCommandParser
SkillCoordinator → SkillExecutor
SkillCoordinator → SkillCameraShake
SkillCoordinator → HitStopController
SkillCoordinator → SkillResultView
SkillCoordinator → PlayerHealth
```

확인:

```text
[ ] R 입력 성공 시 STT latency(ms) 로그 출력
[ ] 플레이어 사망 시 R 입력 차단
[ ] 짧은 R 입력 취소 처리 정상
```

---

## 5-4. FeedbackManager

```text
SkillCameraShake
HitStopController
```

필수 연결:

```text
SkillCameraShake → CameraMovement
```

`SkillCameraShake`는 자동으로 Main Camera를 찾는 보정이 있지만, 병합 안정성을 위해 Inspector에 직접 연결하는 것을 권장합니다.

---

## 5-5. AudioManager

```text
BgmController
AudioSource
```

필수 연결:

```text
BgmController → Player Transform
BgmController → Boss Transform
BgmController → Normal BGM
BgmController → Battle BGM
BgmController → AudioSource
```

권장 설정:

```text
Encounter Distance < Exit Distance
AudioSource Loop = On
AudioSource Spatial Blend = 0
```

---

## 5-6. Player

플레이어 오브젝트에 필요한 주요 컴포넌트:

```text
CharacterController
PlayerMovement
DashSkillRunner
SkillExecutor
PlayerHealth
Animator
```

`SkillExecutor` 필수 연결:

```text
Caster → Player Transform
Forward Reference → Main Camera Transform
Player Health → PlayerHealth
Blade Projectile Prefab → Blade.prefab
Storm Prefab → Storm.prefab
Barrier Prefab → Barrier.prefab
Dash Runner → Player의 DashSkillRunner
```

---

## 5-7. Boss / Enemy

보스 오브젝트에 필요한 주요 컴포넌트:

```text
EnemyMovement
EnemyHealth
AttackTrigger
NavMeshAgent
Animator
Collider
```

확인:

```text
[ ] 보스 크기 2배 적용
[ ] 추격 정상
[ ] 공격 사거리 진입 시 멈춰서 공격
[ ] 공격 중 사망 시 즉시 사망 모션 전환
[ ] EnemyHealth가 스킬 피해를 받음
[ ] Enemy HP Slider 연결
```

---

# 6. 조작 키 충돌 확인

현재 입력 키:

| 입력 | 기능 | 비고 |
|---|---|---|
| W / A / S / D | 이동 | PlayerMovement |
| Shift | 달리기 | PlayerMovement |
| Space | 점프 | PlayerMovement |
| Mouse | 카메라 회전 | CameraMovement |
| R | 음성 스킬 입력 | 쿨타임 없음 |
| Z | 단검 키보드 테스트 | 5초 쿨타임 |
| X | 바람 키보드 테스트 | 5초 쿨타임 |
| C | 방패 키보드 테스트 | 5초 쿨타임 |
| V | 돌진 키보드 테스트 | 5초 쿨타임 |

병합 전 확인:

```text
[ ] R 키가 다른 기능과 충돌하지 않는지 확인
[ ] Z/X/C/V 키가 팀 프로젝트 기능과 충돌하지 않는지 확인
[ ] 마우스 카메라 제어가 기존 카메라 시스템과 충돌하지 않는지 확인
[ ] Cursor Lock 처리 방식 확인
```

---

# 7. 현재 제외된 기능

아래 기능은 현재 프로젝트 범위에서 제거되었습니다.

```text
Gemini LLM
Muninn Persona
ConversationCoordinator
PersonaProvider
GeminiClient
PushToTalkInput
SpeechBubbleView
Billboard
config.json
```

병합 시 다시 추가하지 않는 것을 권장합니다.

체크:

```text
[ ] LLM/무닌 관련 구버전 코드가 다시 들어오지 않는지 확인
[ ] 말풍선 UI가 필요하지 않다면 SpeechBubble 계열 재도입 금지
[ ] config.json / API Key 구조 재도입 여부 확인
```

---

# 8. 병합 후 필수 테스트

## 8-1. 이동 / 카메라

```text
[ ] WASD 이동 정상
[ ] Shift 달리기 정상
[ ] Space 점프 정상
[ ] 마우스 카메라 회전 정상
[ ] 플레이어가 바닥 아래로 떨어지지 않음
```

## 8-2. 팀원 전투 시스템

```text
[ ] 보스 크기 2배 정상
[ ] 보스가 플레이어를 감지함
[ ] 보스가 플레이어를 추격함
[ ] 보스가 공격 사거리에서 멈춰 공격함
[ ] 보스가 플레이어를 바라보고 공격함
[ ] 보스 공격 중 사망 시 즉시 사망 모션으로 전환
[ ] 보스 HP UI 정상
[ ] 보스 피격 로그 정상 출력
```

## 8-3. 키보드 스킬

```text
[ ] Z 단검 정상
[ ] X 바람 정상
[ ] C 방패 정상
[ ] V 돌진 정상
[ ] 단검 4개가 좌상 / 우상 / 좌하 / 우하 위치에서 발사됨
[ ] 단검은 같은 적에게 1회만 피해 적용
[ ] 바람은 적을 중심으로 약간 끌어당김
[ ] 바람은 0.3초마다 1 피해 적용
[ ] 방패가 보스 이동을 차단함
[ ] 돌진 꼬리 / 이펙트 정상
```

## 8-4. 키보드 쿨타임 / UI

```text
[ ] Z/X/C/V 각각 5초 쿨타임 적용
[ ] 스킬별 쿨타임 독립 적용
[ ] 쿨타임 중 같은 키 재입력 시 스킬 발동 안 됨
[ ] 쿨타임 중 안내 UI 출력
[ ] 스킬 아이콘 어두운 오버레이 표시
[ ] 쿨타임 숫자 표시
[ ] 5초 후 오버레이 / 숫자 사라짐
```

## 8-5. 음성 스킬

```text
[ ] R 누르고 말하기 가능
[ ] R 떼면 STT 처리
[ ] 단검 / 바람 / 방패 / 돌진 음성 인식 가능
[ ] 음성 스킬은 키보드 쿨타임과 무관하게 발동
[ ] 짧은 R 입력 취소 정상
[ ] 알 수 없는 명령 실패 처리 정상
[ ] STT latency(ms) 로그 출력
```

## 8-6. BGM / UI / 사망 처리

```text
[ ] 시작 시 평시 BGM 재생
[ ] 보스 조우 시 전투 BGM 전환
[ ] 보스와 멀어지면 평시 BGM 복귀
[ ] Player HP UI 우측 하단 유지
[ ] Enemy HP UI 상단 중앙 유지
[ ] SkillGuideGroup 좌측 하단 유지
[ ] Player 사망 후 이동 불가
[ ] Player 사망 후 R/Z/X/C/V 스킬 입력 차단
```

## 8-7. 최종 Console 확인

```text
[ ] Console 빨간 에러 없음
[ ] NullReferenceException 없음
[ ] MissingReferenceException 없음
[ ] Missing Script 없음
[ ] Animator Parameter 경고 없음
[ ] Input System 오류 없음
```

의도된 로그:

```text
Enemy 피격 로그
STT 인식 로그
STT Latency 로그
쿨타임 안내 로그
BGM 전환 관련 로그가 있다면 허용
```

---

# 9. 병합 후 우선 확인할 위험 요소

## 9-1. 카메라 시스템 충돌

현재 프로젝트는 `CameraMovement`를 사용합니다.

스킬 방향은 `SkillExecutor.forwardReference`에 연결된 Transform 기준으로 결정됩니다.

체크:

```text
[ ] Main Camera에 CameraMovement 연결
[ ] SkillExecutor Forward Reference가 Main Camera Transform을 가리킴
[ ] SkillCameraShake가 CameraMovement를 참조함
```

---

## 9-2. Input System 충돌

현재 일부 입력은 `UnityEngine.Input` 기반입니다.

팀 프로젝트가 New Input System만 사용하는 경우, 아래 오류가 날 수 있습니다.

```text
InvalidOperationException: You are trying to read Input using the UnityEngine.Input class...
```

해결 방향:

```text
Project Settings > Player > Active Input Handling
→ Both 또는 Input Manager 사용
```

또는 추후 `PushToSkillInput`, `SkillDebugInput`, `PlayerMovement`를 New Input System 방식으로 변경합니다.

---

## 9-3. TeamMerge 원본 폴더 정리 주의

현재 `Assets/TeamMerge/Teammate/`는 팀원 원본 씬, 프리팹, 사운드, 설정, 전투 스크립트를 포함합니다.

```text
Assets/TeamMerge/Teammate/
├── Prefabs/
├── Scenes/
├── Scripts/
├── Settings/
└── Sounds/
```

현재는 참조 안정성과 원본 비교를 위해 유지하는 것을 권장합니다.

주의:

```text
[ ] main 병합 전 TeamMerge 폴더 임의 삭제 금지
[ ] AoF.unity 원본 씬은 비교 / 백업용으로 보류
[ ] Settings 폴더의 URP 관련 파일은 실제 참조 여부 확인 전 삭제 금지
[ ] Sounds 폴더는 BGM 전환에서 사용 중이므로 삭제 금지
```

---

# 10. 병합 추천 순서

```text
1. merge-teammate-project 브랜치 최신 상태 확인
2. FinMergeScene 최종 기능 테스트
3. README / MERGE_CHECKLIST / FINAL_TEST_CHECKLIST 최신화
4. main 브랜치로 이동
5. main 최신화
6. merge-teammate-project 병합
7. Unity에서 main 기준 FinMergeScene 열기
8. Console 에러 확인
9. 키보드 스킬 테스트
10. 음성 스킬 테스트
11. 보스 전투 / BGM / UI 테스트
12. 정상 확인 후 main push
```

명령어 예시:

```bash
git checkout main
git pull origin main
git merge merge-teammate-project
git push origin main
```

---

# 11. 병합 완료 기준

아래 조건을 만족하면 병합 완료로 봅니다.

```text
[ ] Unity Console 빨간 에러 없음
[ ] Missing Script 없음
[ ] FinMergeScene 정상 로드
[ ] 이동 / 카메라 정상
[ ] 보스 전투 정상
[ ] Z/X/C/V 키보드 스킬 정상
[ ] 키보드 스킬 쿨타임 UI 정상
[ ] R 음성 스킬 정상
[ ] R 음성 스킬은 쿨타임 없이 동작
[ ] STT latency 로그 정상
[ ] 결과 UI 정상
[ ] Hit Stop / Camera Shake 정상
[ ] BGM 전환 정상
[ ] Player 사망 처리 정상
[ ] Whisper 모델 파일 로컬 배치 완료
[ ] main 병합 완료
```
