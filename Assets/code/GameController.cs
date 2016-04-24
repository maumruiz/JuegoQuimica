using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	// Obtiene los game objects de todos los pool
	public GameObject moleculasPool;
	public GameObject primariosPool;
	public GameObject secundariosPool;
	public GameObject terciariosPool;

	// objetos del modal panel
	public GameObject modalPanel;
	public Text textoModal;
	public Image imagenModal;
	public Sprite errorSprite;
	public Sprite successSprite;

	public GameObject ayudaPanel;

	//Prefab de la molecula para crear instancias de ella
	public GameObject moleculaPrefab;
	bool destruir;

	// Veces ganadas
	public Text ganadosText;
	int ganados = 0;

	// transform del pool de moleculas para instanciar los prefabs en ella
	Transform moleculasPoolZone;

	//Dictionary<string, int> moleculas = new Dictionary<string, int>();

	public struct Molecula {
		public string nombre;
		public int tipo;

		public Molecula(string nomb, int tip) {
			nombre = nomb;
			tipo = tip;
		}
	}

	// Lista que contiene las estructuras de moleculas
	// Para intanciarlas al azar en el pool de moleculas
	List<Molecula> moleculas = new List<Molecula>();

	// Bool array para ver si la molecula ya se creó
	bool[] moleculasCreadas = new bool[15];

	//colores solo para testing
	Color rojo = new Color32(200,83,83,255);
	Color azul = new Color32(63,92,200,255);
	Color verde = new Color32(60,170,52,255);




	// Funcion para crear una cantidad de instancias de molecula
	void creaMoleculas(int cantidad) {
		GameObject molecula_clone;
		int rand;

		for (int i = 0; i < cantidad; i++) {
			// Busca un numero de molecula que no haya sido creada
			do {
				rand = Random.Range (0, 15);
			} while (moleculasCreadas [rand]);
			moleculasCreadas [rand] = true;

			// Crea una instancia del prefab de molecula
			molecula_clone = (GameObject)Instantiate(moleculaPrefab);
			molecula_clone.transform.SetParent (moleculasPoolZone, false);
			molecula_clone.GetComponent<InfoMolecula>().nombre = moleculas[rand].nombre;
			molecula_clone.GetComponent<InfoMolecula>().tipo = moleculas[rand].tipo;


			if (molecula_clone.GetComponent<InfoMolecula> ().tipo == 1) {
				molecula_clone.GetComponent<Image> ().color = rojo;
			} else if (molecula_clone.GetComponent<InfoMolecula> ().tipo == 2) {
				molecula_clone.GetComponent<Image> ().color = azul;
			} else {
				molecula_clone.GetComponent<Image>().color = verde;
			}

				
			
		}
	}

	// Inicio del juego
	void Start () {
		moleculasPoolZone = moleculasPool.transform;

		moleculas.Add(new Molecula ("molecula1", 1));
		moleculas.Add(new Molecula ("molecula2", 1));
		moleculas.Add(new Molecula ("molecula3", 1));
		moleculas.Add(new Molecula ("molecula4", 1));
		moleculas.Add(new Molecula ("molecula5", 1));
		moleculas.Add(new Molecula ("molecula6", 2));
		moleculas.Add(new Molecula ("molecula7", 2));
		moleculas.Add(new Molecula ("molecula8", 2));
		moleculas.Add(new Molecula ("molecula9", 2));
		moleculas.Add(new Molecula ("molecula10", 2));
		moleculas.Add(new Molecula ("molecula11", 3));
		moleculas.Add(new Molecula ("molecula12", 3));
		moleculas.Add(new Molecula ("molecula13", 3));
		moleculas.Add(new Molecula ("molecula14", 3));
		moleculas.Add(new Molecula ("molecula15", 3));

		creaMoleculas (8);
	}


	// Metodo para checar las moleculas de adentro de un drop zone
	bool checaDropZones(Transform dropZone, int tipoDropZone) {
		string nombre_hijo;
		int tipo_hijo;

		foreach (Transform child in dropZone) {
			//nombre = child.GetComponent<InfoMolecula> ().nombre;
			tipo_hijo = child.GetComponent<InfoMolecula> ().tipo;

			if (tipo_hijo != tipoDropZone) {
				return false;
			}
		}
		return true;
	}

	// Verifica los tipos de alcohol cuando se le da click al boton
	public void verificaTipos () {
		//Debug.Log (primariosPool.transform.childCount);
		//Debug.Log (secundariosPool.transform.childCount);
		//Debug.Log (terciariosPool.transform.childCount);

		modalPanel.SetActive (true);
		textoModal.text = "Muy bien!";
		imagenModal.sprite = errorSprite;


		if (moleculasPool.transform.childCount > 0) {
			// enseña modal de que tiene que usar todas las moleculas
			Debug.Log ("Necesitas clasificar todas las moleculas!");
			textoModal.text = "Necesitas clasificar todas las moleculas!";
			imagenModal.sprite = errorSprite;
		}
		else if (!checaDropZones (primariosPool.transform, 1)) {
			//Enseña modal de que hay error en alcoholes primarios
			Debug.Log ("Error en los primarios!");
			textoModal.text = "Error en los alcoholes primarios!";
			imagenModal.sprite = errorSprite;
		} else if (!checaDropZones (secundariosPool.transform, 2)) {
			//Enseña modal de que hay error en alcoholes secundarios
			Debug.Log ("Error en los secundarios!");
			textoModal.text = "Error en los alcoholes secundarios!";
			imagenModal.sprite = errorSprite;
		} else if (!checaDropZones (terciariosPool.transform, 3)) {
			//Enseña modal de que hay error en alcoholes terciarios
			Debug.Log ("Error en los terciarios!");
			textoModal.text = "Error en los alcoholes terciarios!";
			imagenModal.sprite = errorSprite;
		} else {
			// Enseña modal de correcto!
			Debug.Log("Todos bien!");
			textoModal.text = "Muy bien!";
			imagenModal.sprite = successSprite;

			destruir = true;
			ganados++;
			ganadosText.text = ganados.ToString();
		}
	}

	public void cierraModalPanel() {
		
		modalPanel.SetActive (false);

		int moleculasQuedan = 0;

		if (destruir) {
			foreach (Transform child in moleculasPool.transform) {
				GameObject.Destroy (child.gameObject);
			}
			foreach (Transform child in primariosPool.transform) {
				GameObject.Destroy (child.gameObject);
			}
			foreach (Transform child in secundariosPool.transform) {
				GameObject.Destroy (child.gameObject);
			}
			foreach (Transform child in terciariosPool.transform) {
				GameObject.Destroy (child.gameObject);
			}

			for (int i = 0; i < 15; i++) {
				if (!moleculasCreadas [i])
					moleculasQuedan++;
			}

			if (moleculasQuedan < 8) {
				for (int i = 0; i < 15; i++)
					moleculasCreadas [i] = false;
			}

			creaMoleculas (8);

			if (ganados >= 5) {
				Debug.Log (" regresando a los niveles");
			}

			destruir = false;
		}
	}

	public void abreAyudaPanel() {
		ayudaPanel.SetActive (true);
	}

	public void cierraAyudaPanel() {
		ayudaPanel.SetActive (false);
	}
}
