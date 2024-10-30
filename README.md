# KmsRPG 프로젝트
 ![image](https://github.com/user-attachments/assets/20e28b7d-7533-4af3-88b8-931a88ceeaec)

## 개요
아이템(룬) 기능, 캐릭터에 따른 이동과 공격 변화, 데이터의 정보 암호화 저장(XOR) 등의 게임을 위한 간단한 기능들을 구현한 뱀서라이크 게임 제작 및 출시 프로젝트

### 장르
2D 뱀파이어 서바이벌 라이크

### 인원
1명

### 기간
2023.07 ~ 2023.11(업데이트 2024.01)

## 활용 기술
- 활용 기술
    - 유니티 Coroutine
    - 유니티 리모트
    - 유니티 애니메이터
    - 유니티 스프라이트
    - 구글플레이 콘솔
- 디자인 패턴
    - 싱글톤 패턴
- 객체지향
    - 캡슐화
 
## 출시 이미지
![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/e8b1a47e-643b-4a14-94fd-70d0b7b536f1/image.png)

현재 구글스토어 정책변경으로 어플이 삭제된 상태입니다.

## 시연 화면
로비화면

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/8313be84-43fc-4e63-9554-32d319880b31/image.png)

룬 시스템과 키셋팅(모바일)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/bfda62f5-661f-4b0f-9e64-54acb5c738aa/image.png)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/a30d4010-5e87-4eba-b644-3eff9b4ebc94/image.png)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/27e30606-0ef5-49f0-9e6d-2e87c8d93a9b/image.png)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/eaffc579-cd3d-4023-bd16-ca8574b0ec26/image.png)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/f41a0723-ed10-4ab0-819b-68c6b30959ef/image.png)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/609aef09-5da9-40a7-aabe-d667c774708e/image.png)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/7b664f39-76f6-4abd-9bd3-6d8135a41726/image.png)

모바일 UI적용 - 버튼 (아래 그림)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/3263c0a4-673b-4c92-94bd-92a6c4f35df1/image.png)

조이스틱(아래 그림)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/d63fba87-a0db-40d2-bda3-57328957816f/image.png)

*목수 캐릭터의 경우 벡터방향으로, 엘프는 상하좌우로 이동

## 구현 리스트
### 히트백과 코루틴을 활용한 공격딜레이 구현
- 공격시

![스크린샷 2024-10-29 오후 5.47.55.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/e7033d1e-3b43-4b39-ac13-9193720289d6/%E1%84%89%E1%85%B3%E1%84%8F%E1%85%B3%E1%84%85%E1%85%B5%E1%86%AB%E1%84%89%E1%85%A3%E1%86%BA_2024-10-29_%E1%84%8B%E1%85%A9%E1%84%92%E1%85%AE_5.47.55.png)

- 피격시

![스크린샷 2024-10-29 오후 7.20.03.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/b972175c-dfa2-4bf3-bbe4-850d6e3e98d0/%E1%84%89%E1%85%B3%E1%84%8F%E1%85%B3%E1%84%85%E1%85%B5%E1%86%AB%E1%84%89%E1%85%A3%E1%86%BA_2024-10-29_%E1%84%8B%E1%85%A9%E1%84%92%E1%85%AE_7.20.03.png)

RigidBody2D, Collider2D 컴포넌트를 활용하여 충돌시 AddForce를 통해 반대로 밀려나게 하였습니다. 이때 StopCoroutine을 통해 플레이어의 공격이 캔슬되도록 하였습니다.

### JSON화 및 암호화를 통한 데이터 관리
![스크린샷 2024-10-29 오후 8.26.58.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/b3986a3e-4676-46cf-9f55-f5c2a5ce681c/%E1%84%89%E1%85%B3%E1%84%8F%E1%85%B3%E1%84%85%E1%85%B5%E1%86%AB%E1%84%89%E1%85%A3%E1%86%BA_2024-10-29_%E1%84%8B%E1%85%A9%E1%84%92%E1%85%AE_8.26.58.png)

```csharp
string Encrypt(string s, byte key)
	{
		var bytes = System.Text.Encoding.UTF8.GetBytes(s);
		for (int i = 0; i < bytes.Length; i++)
		{
			bytes[i] = (byte)(bytes[i] ^ key);
		}
		return System.Convert.ToBase64String(bytes);
	}
```

### ‘데미지 상태’ 클래스를 사용하여 데미지 정보 교환
![스크린샷 2024-10-29 오후 5.43.05.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/80f7f591-5359-4946-8d67-843c9b3f6b96/%E1%84%89%E1%85%B3%E1%84%8F%E1%85%B3%E1%84%85%E1%85%B5%E1%86%AB%E1%84%89%E1%85%A3%E1%86%BA_2024-10-29_%E1%84%8B%E1%85%A9%E1%84%92%E1%85%AE_5.43.05.png)

### 캐릭터 애니메이션
무료 에셋을 다운받아 진행하였습니다. 필요한 모션은 직접 가공/수정 하여 추가하였습니다.(아래 그림)

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/55ee9d40-05e1-47e9-a405-6d90bf111db1/image.png)

기본 에셋

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/7c65c7da-c69f-43c1-8f98-a771abf8b82e/image.png)

캐릭터 형태와 기본모션을 다듬고 필요한 모션을 추가하였습니다.

애니메이터와 Blend Tree

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/95c3500a-4da7-4092-be99-9b140f92f58a/image.png)

Idle, Move, Attack, Hurt, Dead 상태로 나누어 상황에 맞게 배치하였습니다.

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/d189d3ee-751f-42e3-9e2a-8dcefc1d58e7/image.png)

Blend Tree를 추가하여 파라미터에 따라 상태에 맞는 애니매이션이 동작하도록 하였습니다. 

![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/53be9ae2-7fb9-424e-9fd5-e833ae3d81b1/image.png)

- 파라미터가 2개인 목수캐릭터의 경우

![스크린샷 2024-10-22 오후 5.30.20.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/60d3f9a5-dc2a-494b-9761-51c082930a96/e9c0ee6b-bf5c-4b9d-b7cf-5c9252f226a7/%E1%84%89%E1%85%B3%E1%84%8F%E1%85%B3%E1%84%85%E1%85%B5%E1%86%AB%E1%84%89%E1%85%A3%E1%86%BA_2024-10-22_%E1%84%8B%E1%85%A9%E1%84%92%E1%85%AE_5.30.20.png)

- 상하좌우 모션이 4개가 있는 엘프캐릭터의 경우, 유사도가 높은 방향으로 적용됩니다.
