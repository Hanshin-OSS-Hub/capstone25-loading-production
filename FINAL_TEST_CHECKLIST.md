# 최종 기능 테스트 결과 기록표

이 문서는 `stt-skill-game` 분할 프로젝트의 최종 기능 테스트 항목을 기록하기 위한 체크리스트입니다.

테스트 기준:

```text
Unity 2022.3.62f3
MainTechnologyScene
Whisper Local STT
STT 기반 스킬 입력 + 키보드 디버그 스킬 입력
```

---

# 1. 이동 / 카메라

```text
[ v ] WASD 이동 정상
[ v ] Shift 달리기 정상
[ v ] Space 점프 정상
[ v ] 마우스 카메라 정상
```

---

# 2. 키보드 스킬 입력

```text
[ v ] Z 단검 정상
[ v ] X 바람 정상
[ v ] C 방패 정상
[ v ] V 돌진 정상
```

---

# 3. 스킬 동작 세부 확인

```text
[ v ] 단검 4개 위치 정상
[ v ] 바람 최소 높이 정상
[ v ] 방패 수평 생성 정상
[ v ] 돌진 수평 이동 정상
```

---

# 4. 피드백 / UI

```text
[ v ] Hit Stop 정상
[ v ] Camera Shake 정상
[ v ] 결과 UI 정상
```

---

# 5. 음성 스킬 입력

```text
[ v ] R 단검 정상
[ v ] R 바람 정상
[ v ] R 방패 정상
[ v ] R 돌진 정상
[ v ] R 짧은 입력 취소 정상
[ v ] 알 수 없는 음성 처리 정상
```

---

# 6. Console / Unity 상태

```text
[ v ] Console 빨간 에러 없음
[ v ] Missing Script 없음
```

---

# 최종 확인 기준

아래 조건을 만족하면 최종 기능 테스트 통과로 봅니다.

```text
이동 / 카메라 정상
키보드 스킬 정상
음성 스킬 정상
스킬 결과 UI 정상
Hit Stop / Camera Shake 정상
Console 빨간 에러 없음
Missing Script 없음
```
