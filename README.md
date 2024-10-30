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
![image](https://github.com/user-attachments/assets/a014ed85-b66e-4269-9090-352af53bf830)

현재 구글스토어 정책변경으로 어플이 삭제된 상태입니다.

## 시연 화면
로비화면
![image](https://github.com/user-attachments/assets/d40fd1bf-a40a-4a21-8168-f753439b504e)
캐릭터 선택 화면
![image](https://github.com/user-attachments/assets/288cf2ed-7ad3-4921-8c49-08c69c23d077)


룬 시스템과 키셋팅(모바일)
![image](https://github.com/user-attachments/assets/72c2f812-d7b7-48dc-a255-bcfff3ac628c)
![image](https://github.com/user-attachments/assets/e8be58e6-e127-4887-ba0d-4557d71951a3)
![image](https://github.com/user-attachments/assets/b06cbff7-0f8f-494d-be4c-73f8b518b932)



![image](https://github.com/user-attachments/assets/e7a56a4d-cc6b-4bb0-b271-303391be5c56)

![image](https://github.com/user-attachments/assets/9c43d777-b326-4fc6-b63a-9a98bbbd34c5)

![image](https://github.com/user-attachments/assets/0c9d83dd-1195-41ff-9325-e16361aaab5c)

모바일 UI적용 - 버튼 (아래 그림)
![image](https://github.com/user-attachments/assets/a81cf48f-c2c2-4ef0-83b5-3b10c5963682)

조이스틱(아래 그림)
![image](https://github.com/user-attachments/assets/0efc0cfe-c76c-4977-a58e-13b426927c7c)


*목수 캐릭터의 경우 벡터방향으로, 엘프는 상하좌우로 이동

## 구현 리스트
### 히트백과 코루틴을 활용한 공격딜레이 구현
- 공격시
![image](https://github.com/user-attachments/assets/745dae7e-040d-492e-aed8-66ca899c4d59)

- 피격시
![image](https://github.com/user-attachments/assets/382ee8b9-26de-4d5e-bea4-c0aafe969c00)

RigidBody2D, Collider2D 컴포넌트를 활용하여 충돌시 AddForce를 통해 반대로 밀려나게 하였습니다. 이때 StopCoroutine을 통해 플레이어의 공격이 캔슬되도록 하였습니다.

### JSON화 및 암호화를 통한 데이터 관리
![image](https://github.com/user-attachments/assets/d77848e3-079a-4570-8c0f-84442d867fa8)


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
![image](https://github.com/user-attachments/assets/498faa3b-19ed-48fe-8c81-b12724aa86d4)

### 캐릭터 애니메이션
무료 에셋을 다운받아 진행하였습니다. 필요한 모션은 직접 가공/수정 하여 추가하였습니다.(아래 그림)
![엘프gif](https://github.com/user-attachments/assets/c95c90af-69a8-48b8-90fd-8975c117835d)

캐릭터 형태와 기본모션을 다듬고 필요한 모션을 추가하였습니다.

애니메이터와 Blend Tree
![image](https://github.com/user-attachments/assets/a0a4f35f-7356-4147-8f45-e0ec18bdd215)

Idle, Move, Attack, Hurt, Dead 상태로 나누어 상황에 맞게 배치하였습니다.
![image](https://github.com/user-attachments/assets/5c8488a8-ab31-4ff6-a7ea-e601837e5e48)

Blend Tree를 추가하여 파라미터에 따라 상태에 맞는 애니매이션이 동작하도록 하였습니다. 
![image](https://github.com/user-attachments/assets/e3cc7378-1d86-49ab-ac6d-2890e92ee5d5)
- 파라미터가 2개인 목수캐릭터의 경우
  
![image](https://github.com/user-attachments/assets/4896baf3-86da-4813-8c3b-f292dd926ab5)
- 상하좌우 모션이 4개가 있는 엘프캐릭터의 경우, 유사도가 높은 방향으로 적용됩니다.
