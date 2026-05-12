# Unity 수박 게임 프로젝트

## 프로젝트 개요

Unity 2D URP 기반의 수박 게임(Suika Game) 클론 프로젝트.

## 문서 참조 지침

**게임 구현 관련 작업을 시작하기 전에 반드시 아래 문서를 읽을 것:**

- [`Docs/SuikaGameMechanics.md`](Docs/SuikaGameMechanics.md) — 수박 게임의 핵심 메카닉, 과일 진화 사이클, 점수 체계, Unity 구현 설계 가이드
- [`Docs/DevRoadmap.md`](Docs/DevRoadmap.md) — 개발 작업 순서 및 현재 진행 상황 (작업 전 반드시 확인, 완료된 항목은 [x]로 업데이트)

## 현재 진행 상황

**Phase 1 완료 → Phase 2 (과일 프리팹 & 물리) 진행 중**

작업을 완료할 때마다 `Docs/DevRoadmap.md`의 해당 항목을 `[O]`로 업데이트하고, 하단 "현재 진행 단계" 섹션도 갱신할 것.

## 기술 스택

- **엔진**: Unity 2D (URP)
- **언어**: C#

## 기본 작업 규칙

`#이슈번호 구현해줘` 요청 시 아래 순서를 자동으로 수행한다:

1. 해당 GitHub 이슈 내용 확인
2. `feature/issue-{번호}-{간단한-설명}` 브랜치 생성
3. 구현 및 Unity MCP로 동작 확인
4. 커밋 & 푸시
5. PR 생성 (이슈 자동 연결 `Closes #번호`)

## 핵심 구현 규칙

- 과일 데이터는 ScriptableObject로 관리
- 과일 물리는 `Rigidbody2D` + `CircleCollider2D` 사용
- 머지 판정은 `OnCollisionEnter2D` 기반
- 동시 충돌 머지 버그(같은 프레임 중복 머지)에 주의할 것
