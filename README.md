# Unity_CustomEditor
 Custom Editor for practice &amp;&amp; personal use

#설명
- 외부 프로그램에서 모델링을 가져오는 중에 반복 작업이 싫어서 커스텀 에디터 제작
- 예) 외부 프로그램의 메테리얼이 넘어와지지 않아, 유니티내에서 직접 메테리얼을 하나하나 다시 넣어줘야 된다.
- 메테리얼을 모델링 메시랜더러에 할당하는 기능
- 메테리얼의 메인 텍스쳐를 외부 프로그램과 동일하게 맞추는 기능 (조건 : 텍스쳐 경로가 Tex\ 또는 tex/여야 한다.)
- 그 외 메테리얼 셋팅은 작업자가 손수 만져야 한다. ( 아직 샘플링된 메테리얼이 없다. )

#절차

- 1.임포트한 모델링을 유니티 씬 안에 배치
- 2.해당 오브젝트에 MaterialSetting 컴포넌트 추가 ( 작업하다 귀찮다 생각되면 윈도우 창으로 변경 가능 )
- 3.해당 컴포넌트 작동 법
-  a. Model에 셋팅할 모델 할당
-  b. MeshRenders Setting 버튼 클릭 ( MeshRenders 프로퍼티에 자동으로 값 추가 )
-  c. Materials에 MeshRenders의 개수와 똑같은 개수의 Material 할당
-  d. Matreials setting 버튼 클릭 

- 4.텍스쳐 셋팅
-  a. Window -> MaterialTexSetting(이후부터 커스텀 에디터로 지칭) 클릭
-  b. StreamingAssets 폴더에 텍스트 파일 새로 생성
-  c. 해당 텍스트 파일에 외부 프로그램의 텍스쳐 셋팅 값을 전부 긁어서 입력
-  d. 커스텀 에디터에 TextFileName에 텍스트 파일 이름 넣기. readme.txt면 readme.txt 전부 입력
-  e. Read Text Test 버튼 클릭 ( 파일 읽기 완료가 로그로 출력되면 정상 작동, Data List 프로퍼티에 파싱된 값이 넣어졌는지 확인 )
-  f. Material List에 3번에서 만든 메테리얼들을 전부 드래그 해서 드롭하기 ( Data List와 Material List의 개수가 같아야 정상이다 )
-  g. Texture List에 외부 프로그램에서 가져온 텍스트 전부 드래그 해서 드롭하기
-  h. Material Tex Setting 버튼 클릭 ( 완료 로그가 뜨면 정상, 오류 로그 발생하면 해당 지침에 따라 행동 )

11/28일 기준 62개 자동셋팅까지 확인
