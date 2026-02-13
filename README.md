# SCOPIQUE(스코피크)

Unity-based First-Person Exploration Simulation  
Keyboard Navigation | Gaze-Triggered System | Reactive Environment

---
## Demo Video
[Watch here] 

---

## Overview

SCOPIQUE는 ‘바라보는 행위와 보여지는 행위의 관계’라는 의미의 형용사로,
인터랙션 구조로 구현한 Unity 기반 개인 프로젝트입니다.

플레이어는 어두운 실내에서 시작하여  
외부 자연 공간과 도심으로 이동합니다.  

공간은 점차 확장되지만,  
그 확장은 단순한 물리적 이동이 아니라  
‘응시의 확대’에 대한 서사적 구조를 내포합니다.

---

## Concept

이 프로젝트는 다음 질문에서 출발했습니다.

우리는 타자의 시선을 인식할 때 왜 공포를 느끼는가?  
그리고 그 감정은 단지 위협뿐일까?

응시는 주체를 노출시키고 불안하게 만듭니다.  
그러나 동시에, 나를 바라보는 타자의 존재는  
이 고립된 세계 속에서 나의 존재를 증명해주는 조건이기도 합니다.

SCOPIQUE는 이러한  
**공포와 쾌락이 공존하는 응시의 양가성**을  
게임 시스템 안에서 구현하고자 했습니다.

---

## Spatial Structure

맵은 총 세 개의 영역으로 구성됩니다.

1. 실내 공간  
   침실 → 거실  

2. 외부 공간  
   산책로(자연) → 중간 다리  

3. 도심 공간  
   도심 거리 → 분수대  

공간은 점차 개방되고 확장됩니다.  

내적 공간에서 외적 공간으로의 이동은  
단순한 환경 변화가 아니라  
‘타자의 가능성’이 확대되는 구조를 의미합니다.

---

## Final Object

최종 지점에는  
분수대 위의 잘린 머리 조각상이 배치되어 있습니다.

이 오브제는  
완결되지 못한 주체성을 상징합니다.

플레이어의 시선은 이 조각상에 집중되며  
Vignette 효과를 통해 장면이 마무리됩니다.

---

## Core Interaction System

### 1. Gaze Trigger System

- 특정 오브제를 3초 이상 응시하면 이벤트 발동
- 시선이 조건이 되는 트리거 구조

---

### 2. Eyeball Behavior

- 플레이어 시선에 포착되면 멈춤
- 시선이 벗어나면 다시 움직이며 추격
- 일정 조건에서 Fade-out 및 후퇴

눈알 오브제는  
플레이어를 위협하는 존재이면서 동시에  
플레이어의 존재를 증명하는 장치입니다.

이 추격은 공포를 유발하지만,  
동시에 “누군가 나를 보고 있다”는 감각을 생성합니다.

---

### 3. Event Sequencing

- 카메라 회전
- 멈춤 시간 조절
- 타이밍 기반 연출

응시가 일정 임계값을 넘으면  
장면 전체가 반응하도록 설계했습니다.

---

## Emotional Intention

이 프로젝트는 단순히 공포를 유발하는 것이 아니라,

- 나를 추격하는 눈에 대한 불안
- 그러나 그 시선 속에서 발생하는 기묘한 안도감
- 불완전한 주체가 타자를 통해 성립되는 구조

를 동시에 체험하도록 구성되었습니다.

---

## Technical Implementation

- Keyboard & Mouse 기반 1인칭 이동
- CharacterController & NavMesh 기반 경로 처리
- Headbob 및 중력 구현
- EventSystem 기반 시선 감지
- Script 간 연결 구조 설계
- Global Volume 기반 Vignette 효과
- Canvas Fade-in 처리

---

## Project Context

- 개인 프로젝트
- Unity 입문 단계에서 제작
- 1인칭 시뮬레이션 형식
- 개념과 시스템 구조의 연결을 실험한 작업

