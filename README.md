# Unity_CustomEditor
 Custom Editor for practice &amp;&amp; personal use

#개발 과정 설명
- 외부 프로그램에서 모델링을 가져오는 중에 마테리얼 셋팅 값이 날라가는 문제가 발생. [ Alembic 형식 사용 ]
- 해결법 : 유니티 내에서 마테리얼에 텍스쳐를 수동으로 넣으면 해결 가능.
- [ 텍스쳐 외의 반사 값 등은 호화된지 않아 수동 조절 필요 ]

#구현된 기능 간략 설명
- 모델의 모든 메시렌더러를 뽑아내는 기능
- 뽑아낸 메시 랜더러에 유저가 생성한 Material을 할당하는 기능
- 텍스쳐 할당 기준 데이터 : 수동으로 외부 프로그램인 PMX Editor에서 .pmx 파일의 텍스쳐 값을 뽑아서 스트리밍 에셋 폴더에 넣기.
- 텍스쳐 할당 기준 데이터에 맞춰 Material에 텍스쳐를 할당하는 기능
- 2달 사용동안 .PMX 캐릭터와 .PMX 스테이지 변환에 문제 없음

#절차

1. 임포트한 모델을 유니티 씬 안에 배치
2. CustomTools의 MaterialSetting 클릭

3. MaterialSetting 방법

![image](https://user-images.githubusercontent.com/66342017/220447135-62189382-7f62-4ca3-bf42-d78a031f5746.png)


* a. AlembicModel에 씬에 배치한 모델 파일을 넣기(드래그 앤 드랍, 클릭 등)
* b. Read All MeshRenders 버튼 클릭 ( MeshRender List의 값이 변경하였으면 정상 작동 )
* c. Material List에  MeshRender List의 개수와 똑같은 개수의 Material을 만들어서 넣기
* d. Setting Start 버튼 클릭하면 끝

4. 텍스쳐 셋팅

![image](https://user-images.githubusercontent.com/66342017/220449378-69bc629f-4e22-4634-abff-825c8cc39be5.png)

* a. CustomTools의 TextureSetting 클릭
* b. StreamingAssets 폴더에 텍스트 파일 새로 생성
* c. 해당 텍스트 파일에 외부 프로그램(pmx editor)의 텍스쳐 셋팅 값을 전부 긁어서 입력 
* d. 커스텀 에디터에 TextFileName에 텍스트 파일 이름 입력. ( 텍스쳐.txt면 확장자까지 전부 입력{ 텍스쳐.txt } )
* e. Read Text Test 버튼 클릭 ( Data List 우측 값이 변경 되었으면 완료 )
* f. Material List에 3-C에서 만든 메테리얼들을 전부 드래그 해서 드롭하기 ( Data List와 Material List의 개수가 같아야 정상이다 )
* g. Texture List에 외부 프로그램에서 가져온 텍스쳐를 전부 드래그 해서 드롭하기 (.jpg와 .png 파일만 정상 작동)
* h. Material Tex Setting 버튼 클릭 ( 완료 로그가 뜨면 정상, 오류 로그 발생하면 해당 지침에 따라 행동 )

02/22일까지 정상 작동 확인

#에디터 사용 Project 결과물 및 느낌점
1. Test_1 임포트된 모델은 그림자, 빛 반사 등등 HDRP의  기능 작동 
* A. Performance && Quality 셋팅. 실시간 O
![image](https://user-images.githubusercontent.com/66342017/204307961-8a6ccdcb-2e12-40a7-8414-a6514d0d42af.png)
* B. Quality 셋팅. Pre-release 기술 적용. 실시간 x. ( 적용기술 Beta 메모리 누수 이슈로 중단하여 밝기 조절이 안되어있습니다. 차후 메모리 누수가 픽스되면 재진행 예정 )
![image](https://user-images.githubusercontent.com/66342017/204301894-914c2ba1-d9c2-463a-9055-4c66d93d7841.png)

#개발 예정
1. 현재는 마테리얼 등의 디자인 지식 부족으로 중지 { 디자인보다 개발에 집중 예정 }
2. 차후 생활이 안정화되면 피부 프리셋 등의 마테리얼 프리셋을 추가할 예정

* 코딩은 에디터 제작과 일부 오픈소스 수정 외에는 X
* 다른 외부 3D 프로그램과 유니티 간의 데이터 교환에 대한 지식 일부 습득
* 게임 안에서의 표현과 메테리얼, 조명, 필터 등의 도메인 지식 증진
* 배포를 위해 정리된 코드들을 보며 코드 정리 일부 학습
* 해당 프로젝트는 지속 및 개선 예정
