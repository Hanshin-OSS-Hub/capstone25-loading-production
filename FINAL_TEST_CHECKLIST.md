# 최종 기능 테스트 결과 기록표

이 문서는 `stt-skill-game` 프로젝트의 최종 메인 씬 기능 테스트 항목을 기록하기 위한 체크리스트입니다.

테스트 기준:

```text
Unity 2022.3.62f3
MainGame
Whisper Local STT
팀원 제공 AoF 씬 기반 전투 시스템
STT 기반 음성 스킬 입력 + 키보드 스킬 입력
스킬 쿨타임 UI + BGM 전환 + STT latency 로그
```

최종 실행 씬:

```text
Assets/Scenes/MainGame.unity
```

이전 테스트 씬과 팀원 원본 씬은 아래에 보관합니다.

```text
Assets/Scenes/Archive/
```

---

# 1. 씬 / Unity 기본 상태

```text
[ v ] MainGame 정상 로드
[ v ] Play 실행 가능
[ v ] Console 빨간 에러 없음
[ v ] Missing Script 없음
[ v ] NullReferenceException 없음
[ v ] MissingReferenceException 없음
[ v ] Animator Parameter 경고 없음
[ v ] Input System 오류 없음
[ v ] Archive 씬이 아니라 MainGame을 기준으로 테스트 중인지 확인
```

---

# 2. 이동 / 카메라

```text
[ v ] WASD 이동 정상
[ v ] Shift 달리기 정상
[ v ] Space 점프 정상
[ v ] 마우스 카메라 회전 정상
[ v ] 플레이어가 바닥 아래로 떨어지지 않음
[ v ] 카메라가 과하게 튀지 않음
[ v ] 사망 전까지 이동 입력 정상
```

---

# 3. 보스 전투 시스템

```text
[ v ] 보스 몬스터 크기 2배 정상
[ v ] 보스가 플레이어를 감지함
[ v ] 보스가 플레이어를 추격함
[ v ] 보스가 공격 사거리 안에서 멈춤
[ v ] 보스가 플레이어를 바라보고 공격함
[ v ] 공격 후 다시 추격 가능
[ v ] 보스 HP UI 표시 정상
[ v ] 보스 피격 로그 정상 출력
[ v ] 보스 공격 중 사망 시 공격 모션을 끊고 즉시 사망 모션으로 전환
[ v ] 보스 사망 후 다시 서 있지 않음
```

---

# 4. 키보드 스킬 입력

```text
[ v ] Z 단검 정상
[ v ] X 바람 정상
[ v ] C 방패 정상
[ v ] V 돌진 정상
```

---

# 5. 스킬 동작 세부 확인

## 5-1. 단검

```text
[ v ] 단검 4개가 좌상 / 우상 / 좌하 / 우하 위치에서 발사됨
[ v ] 단검이 카메라 조준 방향으로 발사됨
[ v ] 여러 단검이 같은 Enemy에 맞아도 1회만 피해 적용
[ v ] 단검 피해량 3 적용
[ v ] Enemy HP 감소
[ v ] Enemy 피격 로그 출력
```

## 5-2. 바람

```text
[ v ] 바람이 조준 방향 앞쪽에 생성됨
[ v ] 바람 최소 높이 정상
[ v ] 바람이 원형 구체 형태로 보임
[ v ] Enemy가 바람 중심으로 약간 끌려감
[ v ] 바람이 0.3초마다 1 피해 적용
[ v ] Enemy HP 감소
[ v ] Enemy 피격 로그 출력
```

## 5-3. 방패

```text
[ v ] 방패가 플레이어 앞에 수평 생성됨
[ v ] 방패가 Enemy 이동을 막음
[ v ] Enemy가 방패를 통과하지 못함
[ v ] 방패가 일정 시간 후 사라짐
[ v ] 방패 사라진 뒤 Enemy 이동 정상
```

## 5-4. 돌진

```text
[ v ] 돌진 방향 정상
[ v ] 돌진 수평 이동 정상
[ v ] 돌진 이펙트 표시
[ v ] 돌진 충돌 피해 없음
[ v ] 돌진 후 이동 정상 복귀
```

---

# 6. 키보드 스킬 쿨타임 / 아이콘 UI

```text
[ v ] SkillGuideGroup이 왼쪽 하단에 잘 배치됨
[ v ] 텍스트 UI 하단에 스킬 아이콘 4개가 가로로 배치됨
[ v ] Z/X/C/V 아이콘 순서가 맞음
[ v ] 아이콘 크기가 적당함
[ v ] Z 사용 후 단검 아이콘 쿨타임 표시
[ v ] X 사용 후 바람 아이콘 쿨타임 표시
[ v ] C 사용 후 방패 아이콘 쿨타임 표시
[ v ] V 사용 후 돌진 아이콘 쿨타임 표시
[ v ] 쿨타임 중 어두운 오버레이 표시
[ v ] 쿨타임 숫자 표시
[ v ] 5초 후 오버레이 / 숫자 사라짐
[ v ] 쿨타임 중 같은 키 재입력 시 스킬 발동 안 됨
[ v ] 쿨타임 중 안내 UI 출력
[ v ] 각 스킬 쿨타임이 독립적으로 적용됨
```

---

# 7. 음성 스킬 입력

```text
[ v ] R 누르고 말하기 가능
[ v ] R 떼면 STT 처리
[ v ] R 단검 정상
[ v ] R 바람 정상
[ v ] R 방패 정상
[ v ] R 돌진 정상
[ v ] R 짧은 입력 취소 정상
[ v ] 알 수 없는 음성 처리 정상
[ v ] 키보드 쿨타임 중이어도 R 음성 스킬은 발동
[ v ] R 음성 스킬 사용이 키보드 쿨타임을 새로 시작하지 않음
```

---

# 8. STT latency 로그

성공한 음성 스킬 시전 후 Console에 아래 형식의 로그가 출력되어야 합니다.

```text
[STT Latency] 단검 | R Press → Skill Cast: 0000 ms, R Release → Skill Cast: 0000 ms
```

체크:

```text
[ v ] R 단검 latency 로그 출력
[ v ] R 바람 latency 로그 출력
[ v ] R 방패 latency 로그 출력
[ v ] R 돌진 latency 로그 출력
[ v ] 실패 입력에서는 latency 로그가 출력되지 않아도 됨
```

---

# 9. BGM 전환

```text
[ v ] 시작 시 평시 BGM 재생
[ v ] 보스에게 가까이 가면 전투 BGM 재생
[ v ] 전환 시 페이드가 자연스러움
[ v ] 보스에게서 멀어지면 평시 BGM 복귀
[ v ] 거리 경계에서 BGM이 계속 깜빡이며 바뀌지 않음
```

---

# 10. UI 앵커 / 배치

```text
[ v ] Enemy HP UI가 상단 중앙에 유지됨
[ v ] Player HP UI가 오른쪽 하단에 유지됨
[ v ] SkillGuideGroup이 왼쪽 하단에 유지됨
[ v ] 해상도 변경 시 UI 위치가 크게 틀어지지 않음
[ v ] UI끼리 겹치지 않음
```

---

# 11. 플레이어 사망 상태

```text
[ v ] Player HP 감소 정상
[ v ] Player HP가 0이 되면 사망 처리
[ v ] 사망 애니메이션 정상
[ v ] 사망 후 이동 불가
[ v ] 사망 후 Z/X/C/V 스킬 사용 불가
[ v ] 사망 후 R 길게 입력해도 스킬 사용 불가
[ v ] 사망 후 R 짧게 입력해도 사망 메시지 우선 출력
```

---

# 12. 피드백 / 결과 UI

```text
[ v ] SkillResultView 결과 UI 정상 표시
[ v ] 스킬 성공 메시지 정상
[ v ] 스킬 실패 메시지 정상
[ v ] STT 인식 결과 메시지 정상
[ v ] Hit Stop 정상
[ v ] Camera Shake 정상
```

---

# 13. 폴더 구조 / 참조 확인

```text
[ v ] 최종 씬은 Assets/Scenes/MainGame.unity 기준
[ v ] 이전 테스트 씬은 Assets/Scenes/Archive/에 보관
[ v ] 전투 스크립트는 Assets/Scripts/Combat/ 기준
[ v ] 사운드 파일은 Assets/Sounds/ 기준
[ v ] URP / 렌더링 설정은 Assets/Settings/ 기준
[ v ] 프리팹 / 머티리얼 / 텍스처 참조 누락 없음
[ v ] 스킬 UI 이미지 정상 표시
[ v ] BGM / SFX 참조 정상
```

---

# 14. 최종 Console 확인

```text
[ v ] Console 빨간 에러 없음
[ v ] NullReferenceException 없음
[ v ] MissingReferenceException 없음
[ v ] Missing Script 없음
[ v ] Animator Parameter 경고 없음
[ v ] Input System 오류 없음
```

허용 가능한 의도된 로그:

```text
Enemy 피격 로그
STT 인식 로그
STT Latency 로그
쿨타임 안내 로그
스킬 성공 / 실패 로그
```

---

# 최종 확인 기준

아래 조건을 만족하면 최종 기능 테스트 통과로 봅니다.

```text
MainGame 정상 로드
이동 / 카메라 정상
보스 전투 정상
키보드 스킬 정상
키보드 스킬 쿨타임 UI 정상
음성 스킬 정상
음성 스킬은 쿨타임 없이 동작
STT latency 로그 정상
보스 조우 BGM 전환 정상
UI 앵커 / 배치 정상
Player 사망 처리 정상
Hit Stop / Camera Shake 정상
폴더 구조와 참조 정상
Console 빨간 에러 없음
Missing Script 없음
```
