# Unity_CustomEditor
 Custom Editor for practice &amp;&amp; personal use

#설명
- 외부 프로그램에서 모델링을 가져오는 중에 반복 작업이 싫어서 커스텀 에디터 제작
- 예) 외부 프로그램의 메테리얼이 넘어와지지 않아, 유니티내에서 직접 메테리얼을 하나하나 다시 넣어줘야 된다.
- 메테리얼을 모델링 메시랜더러에 할당하는 기능
- 메테리얼의 메인 텍스쳐를 외부 프로그램과 동일하게 맞추는 기능 (조건 : 텍스쳐 경로가 Tex\ 또는 tex/여야 한다.)
- 그 외 메테리얼 셋팅은 작업자가 손수 만져야 한다. ( 아직 샘플링된 메테리얼이 없다. )

#절차

1. 임포트한 모델링을 유니티 씬 안에 배치
2. 해당 오브젝트에 MaterialSetting 컴포넌트 추가 ( 작업하다 귀찮다 생각되면 윈도우 창으로 변경 가능 )
3. 해당 컴포넌트 작동 법
* a. Model에 셋팅할 모델 할당
* b. MeshRenders Setting 버튼 클릭 ( MeshRenders 프로퍼티에 자동으로 값 추가 )
* c. Materials에 MeshRenders의 개수와 똑같은 개수의 Material 할당
* d. Matreials setting 버튼 클릭 

4. 텍스쳐 셋팅
* a. Window -> MaterialTexSetting(이후부터 커스텀 에디터로 지칭) 클릭
* b. StreamingAssets 폴더에 텍스트 파일 새로 생성
* c. 해당 텍스트 파일에 외부 프로그램의 텍스쳐 셋팅 값을 전부 긁어서 입력
* d. 커스텀 에디터에 TextFileName에 텍스트 파일 이름 넣기. readme.txt면 readme.txt 전부 입력
* e. Read Text Test 버튼 클릭 ( 파일 읽기 완료가 로그로 출력되면 정상 작동, Data List 프로퍼티에 파싱된 값이 넣어졌는지 확인 )
* f. Material List에 3번에서 만든 메테리얼들을 전부 드래그 해서 드롭하기 ( Data List와 Material List의 개수가 같아야 정상이다 )
* g. Texture List에 외부 프로그램에서 가져온 텍스트 전부 드래그 해서 드롭하기
* h. Material Tex Setting 버튼 클릭 ( 완료 로그가 뜨면 정상, 오류 로그 발생하면 해당 지침에 따라 행동 )

11/28일 기준 62개 자동셋팅까지 확인

#에디터 사용 Project 결과물 및 느낌점
1. Test_1 임포트된 모델은 그림자, 빛 반사 등등 HDRP의  기능 작동 
* A. Performance && Quality 셋팅. 실시간 O
![image](https://user-images.githubusercontent.com/66342017/204307961-8a6ccdcb-2e12-40a7-8414-a6514d0d42af.png)
* B. Quality 셋팅. 추가 기술 적용. 실시간 x. ( 적용기술 Beta 메모리 누수 이슈로 중단하여 밝기 조절이 안되어있습니다. 차후 메모리 누수가 픽스되면 재진행 예정 )
![image](https://user-images.githubusercontent.com/66342017/204301894-914c2ba1-d9c2-463a-9055-4c66d93d7841.png)


* 코딩은 에디터 제작과 일부 오픈소스 수정 외에 X
* 다른 외부 3D 프로그램과 유니티 간의 데이터 교환에 대한 지식 일부 습득
* 게임 안에서의 표현과 메테리얼, 조명, 필터 등의 도메인 지식 증진
* 배포를 위해 정리된 코드들을 보며 코드 정리 일부 학습
* 해당 프로젝트는 지속 및 개선 예정
