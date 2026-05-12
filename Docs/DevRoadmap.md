# 수박 게임 개발 작업 순서

## 진행 상황 범례

- [ ] 미완료
- [O] 완료
- [~] 진행 중

---

## Phase 0. 프로젝트 세팅

- [O] Unity 2D URP 프로젝트 생성
- [O] GitHub 연동 및 초기 push
- [O] .gitignore 설정
- [O] Docs 폴더 및 기획 문서 작성 (SuikaGameMechanics.md, DevRoadmap.md)
- [O] CLAUDE.md 작성 (Claude 작업 지침)

---

## Phase 1. 데이터 구조 설계

- [O] `FruitData` ScriptableObject 정의
  - 레벨(0~10), 이름, 스프라이트, 색상, 반지름, 머지 점수, 드롭 가능 여부
- [O] 11종 과일 ScriptableObject 에셋 생성 (`FruitDataGenerator` 에디터 유틸로 자동 생성)
- [O] `FruitDatabase` ScriptableObject (`GetFruitByLevel`, `GetDroppableFruits` 헬퍼 포함)

---

## Phase 2. 과일 프리팹 & 물리

- [O] 과일 컨테이너(통) 제작
  - 바닥 + 좌우 벽 `EdgeCollider2D` (폭 6.5 / 높이 9.0 units)
  - `FruitPhysics.physicsMaterial2D` (friction=0.4, bounciness=0.1)
  - `LineRenderer` 갈색 테두리, 카메라 orthoSize 조정
- [O] `Fruit.cs` 스크립트 작성
  - `FruitData` 참조 및 비주얼 초기화 (sprite / color, radius)
  - `OnCollisionEnter2D` 기반 머지 판정
  - `isMerging` 플래그로 동시 충돌 중복 머지 방지
  - `MergeRequested` 정적 이벤트 발행 (Phase 3 MergeManager가 구독)
- [O] 과일 베이스 프리팹 생성 (`Assets/Prefabs/Fruit.prefab`)
  - `SpriteRenderer` / `Rigidbody2D`(Interpolate·Continuous) / `CircleCollider2D` / `Fruit.cs`
- [O] 11종 과일 프리팹 완성 (`FruitPrefabGenerator` 에디터 유틸로 자동 생성)
  - 실제 과일 스프라이트 연결 완료 (`Assets/Sprites/`)

---

## Phase 3. 머지 로직

- [ ] `MergeManager.cs` 작성
  - `Fruit.MergeRequested` 이벤트 구독
  - 두 과일 중간점에 다음 레벨 과일 생성 (`FruitDatabase` 참조)
  - 원본 두 과일 Destroy
  - 점수 추가 이벤트 발행 (`OnScoreAdded`)
- [ ] 수박(레벨 10) + 수박 → 둘 다 소멸 (보너스 점수)
- [O] 동시 충돌 버그 방지 — `isMerging` 플래그 (Fruit.cs Phase 2에서 구현 완료)
- [ ] 테스트용 스페이스바 드롭 (`TestDropper.cs`)
  - 스페이스바 입력 시 컨테이너 상단 중앙에서 드롭 가능한 5종 중 랜덤 과일 낙하
  - Phase 4 DropController 구현 전 머지 로직 검증용 임시 기능

---

## Phase 4. 드롭 컨트롤러

- [ ] `DropController.cs` 작성
  - 마우스/터치 X좌표 추적으로 드롭 위치 결정
  - 클릭/탭 시 현재 과일 낙하
  - 드롭 후 쿨다운 (다음 과일 등장 딜레이)
- [ ] 다음 드롭 과일 랜덤 선택 (5종 중 랜덤)
- [ ] 다음 과일 미리보기 오브젝트 (UI 또는 월드 오브젝트)

---

## Phase 5. 게임 오버 감지

- [ ] 상단 데드라인 트리거 설정 (`BoxCollider2D` isTrigger)
- [ ] `GameOverDetector.cs` 작성
  - 과일이 트리거 라인 위에 일정 시간(약 3초) 이상 머무르면 게임 오버
- [ ] `GameManager.cs`에서 게임 오버 상태 관리

---

## Phase 6. 점수 시스템

- [ ] `ScoreManager.cs` 작성
  - 머지 이벤트 수신 → 점수 누적
  - 최고 점수(PlayerPrefs) 저장 및 불러오기

---

## Phase 7. UI

- [ ] 현재 점수 표시 텍스트
- [ ] 다음 과일 미리보기 UI
- [ ] 게임 오버 화면 (최종 점수, 재시작 버튼)
- [ ] 최고 점수 표시

---

## Phase 8. 폴리시 (선택)

- [ ] 머지 이펙트 (파티클 또는 애니메이션)
- [ ] 효과음 (드롭, 머지, 게임 오버)
- [ ] 배경 및 컨테이너 아트 적용
- [O] 실제 과일 스프라이트 연결 (Phase 2에서 완료)

---

## 현재 진행 단계

**Phase 2 완료 → Phase 3 (머지 로직) 시작 전**
