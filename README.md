# 수박 게임 규칙을 활용한 게임

## 개요

- 명칭 : Go! Go! 20원즈
- 제작 인원 : 1인
- 기간 : 2024.08.12 ~ 2024.08.14
- 제작 동기 : 
 이 게임의 주목적은 팬게임이며, 수박 게임을 규칙을 활용하여 팬게임을 만들면 짧게 즐길 수 있는 컨텐츠로 충분히 활용될 수 있다고 생각하였습니다.
- Github : https://github.com/Sonwon112/SuikaGame.git

## 기획 및 설계 과정

- **용어 정리**:
    - 구슬 : 플레이어가 떨어뜨려서 키워나가야하는 오브젝트로 수박게임에서 과일의 역할을 수행
- **기획** :
    
     기존의 수박게임에서처럼  동일한 단계의 구슬이 충돌시 다음 단계의 구슬로 업그레이드를 진행하고 업그레이드 시 점수가 추가되는 시스템으로 개발하였습니다.
    
     기존의 샘플 프로젝트는 UI, 세팅 등을 신경 쓰지 않고 개발하였는데 처음으로 시작화면, 설정화면을 구성하였고, 사운드 세팅을 구성하여보았습니다.
    
- **설계** :
    
    ![suika_flowchart.png](/readme_image/suika_flowchart.png)
    

## 제작 과정

- 리소스 제작
    - 구슬 :
        - 4명의 스트리머의 구독 뱃지
        - 4명의 스트리머의 방송에서 추출한 얼굴
        - 4명이 다같이 있는 모습으로 구슬의 이미지 제작
        
        ![스크린샷 2024-09-02 163228.png](/readme_image/resource.png)
        
    - 사운드 :
        - 스트리머 방송에서 사운드를 추출하여 동일한 단계의 구슬 충돌 시 발생하는 효과음으로 활용
        - 배경음은 AI를 활용하여 사운드 생성
    - 배경 및 UI 디자인
        - 자체 제작
- 구슬 충돌 이벤트 구문 제작
    
    ```java
    private void OnCollisionEnter2D(Collision2D collision)
        {
    			  // 충돌 대상이 구슬인지 확인
            if(collision.gameObject.tag == "Step")
            {
                GameObject otherStep = collision.gameObject;
                //Debug.Log(otherStep.GetComponent<Step>().isCollide());
                
                // 단계가 동일하고, 이미 충돌하지 않은 대상인지 비교
                if(step == otherStep.GetComponent<Step>().getStep()
                   && !otherStep.GetComponent<Step>().isCollide())
                {
                    // 현재 구슬보다 나중에 플레이 영역에 들어온 구슬을 대상으로 삼게끔하여 두개의 구슬에서 충돌 처리를 막게끔 구현
                    if (index > otherStep.GetComponent<Step>().getIndex()) return;
                    if (NextStep == null) return;
                    Vector2 middlePoint = (transform.position+otherStep.transform.position)/2;
                    // 충돌이 이미 발생한 상태로 변경하여 처리중인 대상들 외의 동일한 단계의 구슬에서 이벤트 발생을 막게끔 구현
                    otherStep.GetComponent<Step>().setCollide(true); 
                    Destroy(otherStep);
                    //Debug.Log(step + ", "+index+", " + NextStep);
    								
    								// 다음 단계 구슬 생성
                    GameObject tmp = 
                    Instantiate(NextStep, new Vector3(middlePoint.x,middlePoint.y,1f), transform.rotation, gameManager.transform);
                    
                    // 새로 생성된 구슬에 대해 초기 설정
                    tmp.GetComponent<Rigidbody2D>().gravityScale = 1;
                    tmp.GetComponent<Step>().setIndex(index);
                    tmp.GetComponent<Step>().PlaySound();
                    tmp.GetComponent<Step>().setPartPlayArea(true);
    		            // 점수 추가
                    gameManager.appendScore(score);
                    
                    Destroy(gameObject);
                }
            }
        }
    ```
    
- 플레이어 조작 구문 제작
    
    ```java
    // A, S 키 입력시 좌우 이동을 하는 구문
    float horizontal = Input.GetAxis("Horizontal");
    
    transform.Translate(Vector3.right*Time.deltaTime*speed*horizontal);
    
    // 바운더리 내에서만 동작하게끔 하는 구문
    if(transform.position.x <= -1* movingBoundary + 0.55f)
    {
         setPos(-1 * movingBoundary + 0.55f);
    }
    if(transform.position.x >= movingBoundary + 0.55f) 
    {
         setPos(movingBoundary + 0.55f);
    }
    
    // 스페이스 바 입력시 구슬을 떨어 트리는 구문
    if (Input.GetButtonDown("Put") && canPut)
    {   
         canPut = false;
         setPopTrue(); // 애니메이션 동작 구문
         spawn.PutStep();
         //gameManager.spawnStep();
    }
    
    ...
    
    // 바운더리 밖으로 벗어나지 못하게 하는 함수
    void setPos(float xVal) {
       Vector3 pos = transform.position;
       pos.x = xVal;
       transform.position = pos;
    }
    ```
    
- GameManager 제작
    
    ```java
       	// 현재 사용할 구슬과 다음에 사용할 구슬을 스폰
       	public void spawnStep()
        {   
            currStepIndex = nextStepIndex == -1 ? Random.Range(0, stepList.Length):nextStepIndex;
            nextStepIndex = Random.Range(0, stepList.Length);
    
            currStepSpawn.spawnStep(stepList[currStepIndex],index);
            nextStepSpawn.destroyNextStepObjet();
            nextStepSpawn.spawnStep(stepList[nextStepIndex],index);
            index++;
        }
        // 구슬이 탈락 영역에 들어왔음을 감지
         private void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log(other);
            if(other.tag == "Step")
            {
                Step tmp = other.GetComponent<Step>();
                if(tmp != null)
                {
                    // 플레이 영역에 들어가지 않았음을 판단 -> 새롭게 생성된 구슬임
                    if (!tmp.getPartPlayArea())
                    {
                        tmp.setPartPlayArea(true);
                    }
                    else
                    {
                        restart = true;
                        start = false;
                        //Time.timeScale = 0f;
                        gameEndTxt.text = scoreTxt.text;
                        GameEndCanvas.SetActive(true);
                    }
                }
            }
        }
    		// 구슬이 탈락 영역에서 벗어났음을 감지
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Step")
            {
                Step tmp = other.GetComponent<Step>();
                if (tmp != null)
                {   
                    // 구슬을 떨어뜨리고 바로 생성시 충돌이 발생하여 구슬이 이상하게 떨어지게 되어서 구슬이
                    // 어느정도 떨어지고 생성되게끔 수정
                    if (tmp.getPartPlayArea())
                    {
                        spawnStep();
                    }
                    
                }
            }
        }
        
    ```
    
- 설정 제작
    
    ```java
     	      if(setting && Input.GetButtonDown("Exit")) { resumeGame(); } // 설정화면에서 Esc 선택시 게임을 이어서 하게끔 작업
            if (restart && Input.GetButtonDown("Exit")) { ExitUI.callStart(); } // 나가기 수행전 버튼 애니메이션 실행
            // 플레이중 esc키 사용시 이어하기, 설정, 나가기 UI 표시
            if (start && Input.GetButtonDown("Exit")) { 
                start = false;
                EscapeCanvas.SetActive(true); 
            }
    				// 게임 탈락시 스페이스를 통해 재시작
            if (restart && Input.GetButtonDown("Put")) { RestartUI.callStart(); }
            // 설명 창에서 스페이스를 통해 시작
            if(!start && Input.GetButtonDown("Put")) { DescriptionUI.callStart(); }
    ```
    
- 오디오 설정

## 결과

- 화면

![스크린샷 2024-09-02 130444.png](/readme_image/playScreen.png)

- 플레이 영상
    - 니야님 플레이영상
    
    [https://www.youtube.com/watch?v=bH61BmWwjMw](https://www.youtube.com/watch?v=bH61BmWwjMw)
    

## 제작하면서 어려웠던 점 및 배운점

- 다중 충돌 처리
    - 동일한 구슬이 3개 이상 충돌의 경우 두 개의 구슬만 합쳐지는 이벤트가 발생해야했습니다.  단순히 동일한 id를 가진 구슬은 합쳐지게 하면 구현이 안되기 때문에 이미 충돌이 발생한 구슬인지를 판단하게 하였습니다.
    
    ```csharp
    if(step == otherStep.GetComponent<Step>().getStep() && !otherStep.GetComponent<Step>().isCollide())
    {
        // 현재 구슬보다 나중에 플레이 영역에 들어온 구슬을 대상으로 삼게끔하여 두개의 구슬에서 충돌 처리를 막게끔 구현
        if (index > otherStep.GetComponent<Step>().getIndex()) return;
        if (NextStep == null) return;
        Vector2 middlePoint = (transform.position+otherStep.transform.position)/2;
        // 충돌이 이미 발생한 상태로 변경하여 처리중인 대상들 외의 동일한 단계의 구슬에서 이벤트 발생을 막게끔 구현
        otherStep.GetComponent<Step>().setCollide(true); 
        Destroy(otherStep);
        //Debug.Log(step + ", "+index+", " + NextStep);
    		
    		// 다음 단계 구슬 생성
        GameObject tmp = 
        Instantiate(NextStep, new Vector3(middlePoint.x,middlePoint.y,1f), transform.rotation, gameManager.transform);
        
        // 새로 생성된 구슬에 대해 초기 설정
        tmp.GetComponent<Rigidbody2D>().gravityScale = 1;
        tmp.GetComponent<Step>().setIndex(index);
        tmp.GetComponent<Step>().PlaySound();
        tmp.GetComponent<Step>().setPartPlayArea(true);
        // 점수 추가
        gameManager.appendScore(score);
        
        Destroy(gameObject);
    }
    ```
    
    - 충돌이 발생하면 Collide 변수를 true로 바꾸어 다중 충돌이 발생하였을 때 더 이상 처리가 일어나지 않게 하였습니다.
- AudioMixer
    - 팬 게임으로 제작을 하였다보니 방송에서 진행을 할 수 있기 때문에 bgm 부분에서는 볼륨 조절과 on/off 제어가 필요하다고 생각하였습니다.
    - Slider를 통해 조절 되는 0~1사이의값을 -80~20 까지의 값으로 매핑 하여 mixer의 값을 조절하고 mixer를 Audio Source의 Output과 연결하여 볼륨 조절이 되게끔 하였습니다.
    
    ```csharp
    public void SetLevel(float slideVal)
        {
            float tmp = remap(slideVal, 0f, 1f, -80f, 20f);
            mixer.SetFloat("MasterVolume", tmp);
        }
    
        public void SetvolumeTxt(float slideVal)
        {
            string txt = Mathf.Round(slideVal * 100f)  + "%";
            volumeText.text = txt;
        }
    
        public void Mute(bool mute)
        {
            if (mute)
            {
                mixer.SetFloat("bgmVolume", currVolume);
            }
            else
            {
                mixer.SetFloat("bgmVolume", -80f);
            }
        }
    ```