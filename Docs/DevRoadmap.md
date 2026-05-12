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

- [ ] 과일 컨테이너(통) 제작
  - 바닥 + 좌우 벽 콜라이더 (`EdgeCollider2D`)
  - `PhysicsMaterial2D` 설정 (마찰/탄성)
  - 시각적 테두리 표현 (LineRenderer 또는 SpriteRenderer)
- [ ] `Fruit.cs` 스크립트 작성
  - `FruitData` 참조 및 비주얼 초기화 (color, radius)
  - `OnCollisionEnter2D` 기반 머지 판정
  - 중복 머지 방지 플래그
- [ ] 과일 베이스 프리팹 생성
  - `SpriteRenderer`
  - `Rigidbody2D` (중력, 마찰, 탄성 설정)
  - `CircleCollider2D` (레벨별 반지름)
  - `Fruit.cs` 연결
- [ ] 11종 과일 프리팹 완성 (FruitData별 color·radius 적용)

---

## Phase 3. 머지 로직

- [ ] `MergeManager.cs` 또는 `Fruit.cs` 내 머지 처리
  - 같은 레벨 감지 시 두 과일 중간점에 다음 레벨 과일 생성
  - 원본 두 과일 Destroy
  - 점수 추가 이벤트 발행
- [ ] 수박(레벨 10) + 수박 → 둘 다 소멸 처리
- [ ] 동시 충돌 버그 방지 (같은 프레임 중복 머지)

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
- [ ] 실제 과일 스프라이트로 교체

---

## 현재 진행 단계

**Phase 1 완료 → Phase 2 (과일 프리팹 & 물리) 진행 중**
