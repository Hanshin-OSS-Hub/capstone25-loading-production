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
[ ] WASD 이동 정상
[ ] Shift 달리기 정상
[ ] Space 점프 정상
[ ] 마우스 카메라 정상
```

---

# 2. 키보드 스킬 입력

```text
[ ] Z 단검 정상
[ ] X 바람 정상
[ ] C 방패 정상
[ ] V 돌진 정상
```

---

# 3. 스킬 동작 세부 확인

```text
[ ] 단검 4개 위치 정상
[ ] 바람 최소 높이 정상
[ ] 방패 수평 생성 정상
[ ] 돌진 수평 이동 정상
```

---

# 4. 피드백 / UI

```text
[ ] Hit Stop 정상
[ ] Camera Shake 정상
[ ] 결과 UI 정상
```

---

# 5. 음성 스킬 입력

```text
[ ] R 단검 정상
[ ] R 바람 정상
[ ] R 방패 정상
[ ] R 돌진 정상
[ ] R 짧은 입력 취소 정상
[ ] 알 수 없는 음성 처리 정상
```

---

# 6. Console / Unity 상태

```text
[ ] Console 빨간 에러 없음
[ ] Missing Script 없음
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
