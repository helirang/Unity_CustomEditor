# Unity_CustomEditor
 Custom Editor for practice &amp;&amp; personal use

#개발 과정 설명
- 문제 발생 : 외부 프로그램에서 모델링을 가져오는 중에 마테리얼 셋팅 값이 날라가는 문제가 발생. [ Alembic 형식 사용 ]
- 해결법 : MMD 데이터를 텍스트롤 추출하여 유니티내에서 분류하여 적용하는 기능 구현

#구현된 기능 간략 설명
- materialSetting 에디터 : 
  - 등록된 모델의 모든 메시렌더러를 조회하여 배열로 저정
  - 모델의 에셋 위치에 새로운 폴더 생성
  - 생성된 폴더에 메시랜더러 배열의 크기만큼 마테리얼 생성
  - 각 메시랜더러의 마테리얼을 생성된 마테리얼로 교체
  
- MMDSettingTransper 에디터 :
  - base 모델의 셋팅을 target 모델에 복사하는 기능
  - 메쉬 렌더러 셋팅 복사 ( 모션 벡터 / 그림자 생성 여부 / 메테리얼 값 )
  - 메쉬 렌더러 갯수 복사 
  - ( base 모델에 없는 메쉬 렌더러가 target 모델에 있을 경우, 해당 오브젝트를 파괴 / 변경 사항 프리팹 저장 기능 )  

- textureSetting 에디터 : 
  - MMD 데이터 텍스트의 자료를 분류하여 저장하는 기능. [ TextAsset 사용 ]
  - 등록된 모델의 모든 메시랜더러의 마테리얼을 뽑아내서 배열에 저장하는 기능
  [ 게임 씬에 배치된 모델이면 material을 사용 / 나머지는 sharedMaterial을 사용 ] 
  - 가공된 MMD 데이터를 활용하여 마테리얼의 설정을 바꾸는 기능. 

# 사용법 

# MaterialSetting 에디터
![image](https://user-images.githubusercontent.com/66342017/232187654-dfae1a51-6c2c-43e5-9013-dff53f6a5664.png)
1. 프로젝트 창에 위치한 모델을 AlembicModel로 드래그 앤 드롭
2. 마테리얼의 기초 쉐이더를 셋팅
3. Read All 버튼 클릭
4. Setting Start 버튼 클릭
5. 모델의 폴더 위치에 새로운 폴더와 마테리얼들이 생성되어 있다.

# MMDSettingTransper 에디터
![image](https://user-images.githubusercontent.com/66342017/232547795-ad1c0595-0a68-49b8-9153-dd6692d271a8.png)
1. 유니티 셋팅이 되어있는 캐릭터를 base 모델에 넣기
2. 데이터를 받을 모델을 target 모델에 넣기
3. Setting Transper 버튼을 클릭

# TextureSetting 에디터
![image](https://user-images.githubusercontent.com/66342017/232188811-0d176203-dadc-4cab-87f4-7826e8f7a126.png)
1. MMD에서 추출한 데이터 텍스트를 Mmd Text File로 드래그 앤 드롭
2. 원하는 데이터 추출 유형에 따라 버튼 클릭
[메인 텍스쳐 추출만 원하면 Onlty Main Texture 버튼 클릭]
[모든 텍스쳐 ( 메인, 스피어 텍스쳐, 톤 텍스쳐 )를 전부 원하면 All 버튼 클릭 ]
3. 모델을 MaterialSetting 하단에 드래그 앤 드롭
4. Read Alembic 버튼 클릭
5. 텍스쳐 셋팅에 표시된 Need Texture List를 보고 필요한 텍스쳐 파일을 TextureList에 전부 드래그 해서 넣기
6. Final 버튼을 클릭


# 에디터 사용 Project 결과물

![image](https://user-images.githubusercontent.com/66342017/232549802-c4de6861-a356-4425-b743-8a4cccd8c9e3.png)


# 개발 예정
1. 현재는 마테리얼 등의 디자인 지식 부족으로 중지 { 디자인보다 개발에 집중 예정 }
2. 차후 생활이 안정화되면 피부 프리셋 등의 마테리얼 프리셋을 추가할 예정

* 다른 외부 3D 프로그램과 유니티 간의 데이터 교환에 대한 지식 일부 습득
* 게임 안에서의 표현과 메테리얼, 조명, 필터 등의 도메인 지식 증진
* 배포를 위해 정리된 코드들을 보며 코드 정리 일부 학습
* 해당 프로젝트는 지속 및 개선 예정
