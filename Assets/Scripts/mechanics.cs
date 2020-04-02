using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mechanics : MonoBehaviour
{
    
    public float flag = 0;
    public bool playPluck = false;
    public List<GameObject> stones;

    private bool move = false;
    private int count = 0;
    private int fruitCount = 0;
    private ParabolaController cont;
    private OSC oscScript;
    private GameObject oscGameObject;
    public List<GameObject> fru;
    [SerializeField] private float speed = 3f;
    [SerializeField] private List<Material> sky;
    [SerializeField] private List<GameObject> vfx;
    private List<GameObject> toDestory;

    private void Awake()
    {
        RenderSettings.skybox = sky[Random.Range(0, sky.Count)];
    }
    void Start()
    {
        //UnityEngine.XR.InputTracking.disablePositionalTracking = true;
        oscGameObject = GameObject.Find("OSC");
        oscScript = oscGameObject.GetComponent<OSC>();
        oscScript.SetAddressHandler("/Spirometer/C", BreathData);
        toDestory = new List<GameObject>();
        
    }


    void BreathData(OscMessage message)
    {
        float breath_value = message.GetFloat(0);
        Debug.Log(breath_value + " breath");
        if (breath_value>=2600)
        {
            flag = 1;
        }
        else if (breath_value <2600 && breath_value>=1300)
        {
            flag = 2;
        }
        else
        {
            flag = 3;
        }
    }



    void Update()
    {
        OVRInput.Update();
        // inhale the stone
        if (Input.GetKeyDown(KeyCode.Space) || OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || flag==1)
        {
            if (count == stones.Count)
            {
                Debug.Log("No more stones left");   
            }
            else 
            {
                //originalObjPosition = obj.transform.position;
                cont = stones[count].GetComponent<ParabolaController>();
                move = true;

            }

        }
        if (move)
        {

            stones[count].transform.position = Vector3.MoveTowards(stones[count].transform.position, this.transform.position, Time.deltaTime * speed);
            vfx[count].SetActive(true);
            vfx[count].transform.GetChild(0).gameObject.SetActive(false);


        }

        if (stones[count] && Vector3.Distance(stones[count].transform.position, transform.position) <= 0.01f)
        {
            //GameObject.Find("Trails").SetActive(false);
        }
        // When the stone has arrived near the player

        if ((Input.GetKeyDown(KeyCode.D) || OVRInput.Get(OVRInput.RawButton.A) || flag==3)  && Vector3.Distance(stones[count].transform.position, this.transform.position)<0.1f && fruitCount<fru.Count)
        {
            vfx[count].transform.GetChild(0).gameObject.SetActive(true);

            move = false;
            GameObject.Find("Trails").GetComponent<ParticleSystem>().Play();
            GameObject point1 = new GameObject();
            GameObject point2 = new GameObject();
            GameObject point3 = new GameObject();
            GameObject root = new GameObject();
            point1.name = "child1";
            point2.name = "child2";
            point3.name = "child3";
            root.name = "parent";
            point1.transform.parent = root.transform;
            point2.transform.parent = root.transform;
            point3.transform.parent = root.transform;
            point1.transform.position = transform.position;
            point3.transform.position = fru[fruitCount].transform.position;
            point2.transform.position=new Vector3((point1.transform.position.x+point3.transform.position.x)/2,point3.transform.position.y+0.3f, (point1.transform.position.z + point3.transform.position.z) /2);
            
            cont.ParabolaRoot = root;
            cont.Speed = 7f;
            cont.Autostart = true;
            cont.Animation = true;
    
            if (!cont.enabled)
            {
                cont.enabled = true;
            }
    
          

        }//When the stone has hit the  fruit
        if (count<stones.Count && stones[count] && Vector3.Distance(stones[count].transform.position, fru[fruitCount].transform.position) < 1f && fruitCount<fru.Count)
        {
            stones[count].transform.GetChild(0).transform.parent = null;
   
            stones[count].GetComponent<Rigidbody>().useGravity = true;
            playPluck = true;
            Destroy(stones[count]);
            //toDestory.Add(vfx[count]);
            for(int i = 0; i < 3; i++)
            {
                GameObject.Find("stoneVFX").transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
            }
            StartCoroutine(destory(vfx[count]));
            count++;
            fru[fruitCount].GetComponent<Rigidbody>().useGravity = true;
            //fru[fruitCount].GetComponent<Rigidbody>().isKinematic = true;
            fruitCount++;

        }




    }
    IEnumerator destory(GameObject go)
    {
        yield return new WaitForSeconds(3);
        Destroy(go);
    }



}
