# 병합 준비 체크리스트

이 문서는 `stt-skill-game` 분할 프로젝트를 팀 프로젝트에 병합하기 전에 확인해야 할 항목을 정리한 문서입니다.

현재 프로젝트의 핵심 범위는 다음과 같습니다.

```text
Whisper Local STT
→ 음성 명령 인식
→ 스킬 명령 파싱
→ 스킬 실행
→ 결과 UI / Hit Stop / Camera Shake 피드백
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
[ v ] 팀 프로젝트 Unity 버전 확인
[ v ] 가능하면 같은 Unity 2022.3 LTS 계열 사용
[ v ] 병합 전 별도 브랜치 생성
```

---

# 2. 병합해야 할 주요 폴더 / 파일

## 2-1. Scripts

아래 폴더는 현재 기능의 핵심입니다.

```text
Assets/Scripts/Debug/
Assets/Scripts/Gameplay/
Assets/Scripts/Skill/
Assets/Scripts/Systems/
Assets/Scripts/UI/
```

핵심 스크립트:

```text
Gameplay/CameraMovement.cs
Gameplay/PlayerMovement.cs
Gameplay/PushToSkillInput.cs

Systems/AudioRecordingData.cs
Systems/MicrophoneRecorder.cs
Systems/OperationResult.cs
Systems/SttService.cs
Systems/SystemErrorType.cs
Systems/SystemMessageProvider.cs
Systems/WhisperTranscriber.cs

Skill/Core/SkillCastResult.cs
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
Debug/ProjectLogger.cs
```

체크:

```text
[ ] 위 스크립트 폴더 전체 복사
[ ] Unity에서 .meta 파일과 함께 이동
[ ] 파일명과 클래스명 일치 확인
[ ] Console 컴파일 에러 없음 확인
```

---

## 2-2. Prefabs

스킬 실행에 필요한 프리팹:

```text
Assets/Prefabs/Blade.prefab
Assets/Prefabs/Storm.prefab
Assets/Prefabs/Barrier.prefab
```

체크:

```text
[ ] Blade 프리팹 포함
[ ] Storm 프리팹 포함
[ ] Barrier 프리팹 포함
[ ] 각 프리팹의 Material / Texture / VFX 참조 누락 없음
[ ] Missing Script 없음
```

---

## 2-3. Materials / VFX / Textures

스킬 프리팹이 참조하는 Material, Texture, VFX도 함께 필요합니다.

현재 프로젝트에는 다음 계열 에셋이 사용됩니다.

```text
Assets/Materials/
Assets/Eric VFX Studio/
```

체크:

```text
[ ] 스킬 프리팹에 연결된 Material 포함
[ ] 방패 이미지 Material 포함
[ ] VFX 관련 Material / Animator / Texture 포함
[ ] 병합 후 프리팹이 분홍색으로 깨지지 않는지 확인
```

---

## 2-4. UI

현재 결과 UI는 `SkillResultView` 기반입니다.

필요 항목:

```text
SkillHUDCanvas
SkillResultView.cs
TextMeshPro
CanvasGroup
Result Text
```

체크:

```text
[ ] SkillHUDCanvas 병합 또는 팀 프로젝트 UI에 재구성
[ ] SkillResultView 컴포넌트 연결
[ ] Root / CanvasGroup / Result Text 연결
[ ] 스킬 발동 시 결과 UI 표시 확인
```

---

## 2-5. Whisper 모델 파일

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
[ ] 팀 프로젝트에도 StreamingAssets 폴더 생성
[ ] Whisper 모델 .bin 파일 직접 추가
[ ] .gitignore에서 *.bin 제외 유지
[ ] STT 실행 시 모델 로딩 오류 없음 확인
```

---

# 3. 씬 병합 구조

현재 권장 Hierarchy 구조:

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

팀 프로젝트에 그대로 옮길 때는 `GameManager` 하위 구조를 기준으로 옮기는 것이 좋습니다.

---

# 4. Manager별 컴포넌트 배치

## 4-1. InputManager

```text
PushToSkillInput
SkillDebugInput
```

필수 연결:

```text
SkillDebugInput → SkillExecutor
SkillDebugInput → SkillCameraShake
SkillDebugInput → HitStopController
SkillDebugInput → SkillResultView
```

---

## 4-2. STTManager

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

## 4-3. SkillManager

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
```

---

## 4-4. FeedbackManager

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

## 4-5. Player / Penguin

플레이어 오브젝트에 필요한 주요 컴포넌트:

```text
CharacterController
PlayerMovement
DashSkillRunner
SkillExecutor
Animator
```

`SkillExecutor` 필수 연결:

```text
Caster → Player Transform
Forward Reference → Main Camera Transform
Blade Projectile Prefab → Blade.prefab
Storm Prefab → Storm.prefab
Barrier Prefab → Barrier.prefab
Dash Runner → Player의 DashSkillRunner
```

---

# 5. 조작 키 충돌 확인

현재 입력 키:

| 입력 | 기능 |
|---|---|
| W / A / S / D | 이동 |
| Shift | 달리기 |
| Space | 점프 |
| Mouse | 카메라 회전 |
| R | 음성 스킬 입력 |
| Z | 단검 디버그 |
| X | 바람 디버그 |
| C | 방패 디버그 |
| V | 돌진 디버그 |

병합 전 팀 프로젝트와 충돌 확인:

```text
[ ] R 키가 다른 기능과 충돌하지 않는지 확인
[ ] Z/X/C/V 디버그 키가 팀 프로젝트 기능과 충돌하지 않는지 확인
[ ] 마우스 카메라 제어가 기존 카메라 시스템과 충돌하지 않는지 확인
[ ] Cursor Lock 처리 방식 확인
```

---

# 6. 현재 제외된 기능

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
[ ] 팀 프로젝트 병합 시 LLM/무닌 관련 구버전 코드가 다시 들어오지 않는지 확인
[ ] 말풍선 UI가 필요하지 않다면 SpeechBubble 계열 재도입 금지
[ ] config.json / API Key 구조 재도입 여부 확인
```

---

# 7. 병합 후 필수 테스트

## 7-1. 이동 / 카메라

```text
[ ] WASD 이동 정상
[ ] Shift 달리기 정상
[ ] Space 점프 정상
[ ] 마우스 카메라 회전 정상
[ ] 플레이어만 이동하고 다른 캐릭터는 움직이지 않음
```

## 7-2. 키보드 스킬

```text
[ ] Z 단검 정상
[ ] X 바람 정상
[ ] C 방패 정상
[ ] V 돌진 정상
[ ] 단검 4개가 좌상 / 우상 / 좌하 / 우하 위치에서 발사됨
[ ] 바람이 조준 방향에 생성됨
[ ] 방패가 수평 방향 기준으로 생성됨
[ ] 돌진이 수평 방향 기준으로 작동함
```

## 7-3. 음성 스킬

```text
[ ] R 누르고 말하기 가능
[ ] R 떼면 STT 처리
[ ] 단검 / 바람 / 방패 / 돌진 음성 인식 가능
[ ] 짧은 R 입력 취소 정상
[ ] 알 수 없는 명령 실패 처리 정상
```

## 7-4. 피드백

```text
[ ] SkillResultView UI 정상 표시
[ ] Hit Stop 정상
[ ] Camera Shake 정상
[ ] Console 빨간 에러 없음
[ ] Missing Script 없음
```

---

# 8. 병합 후 우선 확인할 위험 요소

## 8-1. 카메라 시스템 충돌

현재 프로젝트는 `CameraMovement`를 사용합니다.

팀 프로젝트에 이미 카메라 시스템이 있다면 둘 중 하나를 선택해야 합니다.

```text
선택 A: 현재 CameraMovement 유지
선택 B: 팀 프로젝트 카메라에 맞게 SkillExecutor의 Forward Reference만 연결
```

스킬 방향은 `SkillExecutor.forwardReference`에 연결된 Transform 기준으로 결정됩니다.

---

## 8-2. Input System 충돌

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

## 8-3. Dummy Enemy 전까지 데미지 처리는 임시 상태

현재 단검과 바람은 실제 적 HP를 깎지 않습니다.

현재 상태:

```text
BladeSkillProjectile → 충돌 로그 후 삭제
StormSkillZone → 범위 내 Rigidbody 끌어당김 + 영향 로그
```

병합 이후 Dummy Enemy 단계에서 처리 예정:

```text
EnemyHealth
TakeDamage(int damage)
BladeSkillProjectile에서 충돌 대상 피해 처리
StormSkillZone에서 틱 데미지 처리
```

---

# 9. 병합 추천 순서

```text
1. 팀 프로젝트에서 새 브랜치 생성
2. Scripts 폴더 병합
3. Prefabs / Materials / VFX 병합
4. Whisper 모델 파일 로컬 추가
5. GameManager 하위 Manager 구조 생성
6. Player에 SkillExecutor / DashSkillRunner 연결
7. UI 연결
8. Inspector 참조 연결
9. Console 에러 확인
10. Z/X/C/V 키보드 스킬 테스트
11. R 음성 스킬 테스트
12. Hit Stop / Camera Shake 확인
13. 팀 프로젝트 기존 기능과 충돌 확인
14. 정상 확인 후 커밋
```

---

# 10. 병합 완료 기준

아래 조건을 만족하면 병합 완료로 봅니다.

```text
[ ] Unity Console 빨간 에러 없음
[ ] Missing Script 없음
[ ] 이동 / 카메라 정상
[ ] Z/X/C/V 스킬 정상
[ ] R 음성 스킬 정상
[ ] 결과 UI 정상
[ ] Hit Stop / Camera Shake 정상
[ ] 팀 프로젝트 기존 기능과 충돌 없음
[ ] Whisper 모델 파일 로컬 배치 완료
[ ] 병합 커밋 생성 완료
```
