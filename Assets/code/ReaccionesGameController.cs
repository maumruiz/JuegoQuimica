using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ReaccionesGameController : MonoBehaviour {

	// Obtiene los game objects de todos los pool
	public GameObject productosPool;
	public GameObject productosDrop;

	// transform del pool de moleculas para instanciar los prefabs en ella
	Transform productosPoolTransform;

	// objetos del modal panel
	public GameObject modalPanel;
	public Text textoModal;
	public Image imagenModal;
	public Sprite errorSprite;
	public Sprite successSprite;

	// Panel de ayuda
	public GameObject ayudaPanel;

	//Prefab de la molecula para crear instancias de ella
	public GameObject productoPrefab;
	bool destruir;

	// Imagen de la reaccion
	public Image ReaccionImage;
	public Sprite spriteReaccion1;
	public Sprite spriteReaccion2;
	public Sprite spriteReaccion3;
	public Sprite spriteReaccion4;
	public Sprite spriteReaccion5;

	// Veces ganadas
	public Text ganadosText;
	int ganados = 0;

	// Estructura de la reaccion con sus productos
	public struct Reaccion {
		public string reaccion;
		public List<string> productos;

		public Reaccion(string nomb, List<string> prod) {
			reaccion = nomb;
			productos = prod;
		}
	}

	// Lista que contiene las estructuras de reacciones
	List<Reaccion> reacciones = new List<Reaccion>();

	// Lista que contiene los productos de las reacciones
	List<string> productos = new List<string>();

	// Bool array para ver si el producto ya se creó
	bool[]  productosCreados = new bool[25];
	bool[]  reaccionesCreadas = new bool[5];

	//colores solo para testing
	Color rojo = new Color32(200,83,83,255);
	Color azul = new Color32(63,92,200,255);
	Color verde = new Color32(60,170,52,255);


	// Funcion para crear una cantidad de instancias de producto
	void creaProductos(int cantidad) {
		GameObject producto_clone;
		int rand;
		string productoCreando;

		for (int i = 0; i < cantidad; i++) {
			// Busca un numero de producto que no haya sido creado
			do {
				rand = Random.Range (0, 25);
			} while (productosCreados [rand]);
			productosCreados [rand] = true;

			// Crea una instancia del prefab de molecula
			producto_clone = (GameObject)Instantiate(productoPrefab);
			producto_clone.transform.SetParent (productosPoolTransform, false);

			// Le da los nombres de producto y reaccion a la instancia
			productoCreando = productos[rand];
			producto_clone.GetComponent<InfoProducto>().producto = productoCreando;

			foreach (Reaccion reacc in reacciones) {
				foreach (string prod in reacc.productos) {
					if (prod == productoCreando)
						producto_clone.GetComponent<InfoProducto> ().reaccion = reacc.reaccion;
				}
			}
				

			if (i%2 == 0) {
				producto_clone.GetComponent<Image> ().color = rojo;
			} else {
				producto_clone.GetComponent<Image> ().color = azul;
			}
		}
	}

	void creaReaccion() {
		int rand;
		Sprite imageToPut = spriteReaccion1;

		// Busca un numero de reaccion que no haya sido creado
		do {
			rand = Random.Range (0, 5);
		} while (reaccionesCreadas [rand]);
		reaccionesCreadas [rand] = true;

		Debug.Log (rand);

		switch (rand) {
		case 0:
			imageToPut = spriteReaccion1;
			break;
		case 1:
			imageToPut = spriteReaccion2;
			break;
		case 2:
			imageToPut = spriteReaccion3;
			break;
		case 3:
			imageToPut = spriteReaccion4;
			break;
		case 4:
			imageToPut = spriteReaccion5;
			break;
		}

		ReaccionImage.sprite = imageToPut;

		ReaccionImage.GetComponent<InfoReaccion> ().reaccion = reacciones [rand].reaccion;
		ReaccionImage.GetComponent<InfoReaccion> ().productos = reacciones [rand].productos;

	}

	// Inicio del juego
	void Start () {
		productosPoolTransform = productosPool.transform;
		
		List<string> productosR1 = new List<string>(new string[] {"producto1", "producto2", "producto3", "producto4", "producto5"});
		List<string> productosR2 = new List<string>(new string[] {"producto6", "producto7", "producto8", "producto9", "producto10"});
		List<string> productosR3 = new List<string>(new string[] {"producto11", "producto12", "producto13", "producto14", "producto15"});
		List<string> productosR4 = new List<string>(new string[] {"producto16", "producto17", "producto18", "producto19", "producto20"});
		List<string> productosR5 = new List<string>(new string[] {"producto21", "producto22", "producto23", "producto24", "producto25"});

		productos.AddRange (productosR1);
		productos.AddRange (productosR2);
		productos.AddRange (productosR3);
		productos.AddRange (productosR4);
		productos.AddRange (productosR5);
		
		reacciones.Add(new Reaccion ("reaccion1", productosR1));
		reacciones.Add(new Reaccion ("reaccion2", productosR2));
		reacciones.Add(new Reaccion ("reaccion3", productosR3));
		reacciones.Add(new Reaccion ("reaccion4", productosR4));
		reacciones.Add(new Reaccion ("reaccion5", productosR5));

		creaProductos (8);
		creaReaccion ();
	}


	// Metodo para checar si todos los productos son correctos para la reaccion
	bool checaProductos(Transform productosZone) {
		string reaccionActual = ReaccionImage.GetComponent<InfoReaccion> ().reaccion;
		string reaccionDeProducto;

		foreach (Transform child in productosZone) {
			reaccionDeProducto = child.GetComponent<InfoProducto> ().reaccion;

			if (reaccionActual != reaccionDeProducto) {
				return false;
			}
		}
		return true;
	}

	// Metodo para checar si hay productos correctos en el pool
	bool quedanProductos(Transform productosZone) {
		string reaccionActual = ReaccionImage.GetComponent<InfoReaccion> ().reaccion;
		string reaccionDeProducto;

		foreach (Transform child in productosZone) {
			reaccionDeProducto = child.GetComponent<InfoProducto> ().reaccion;

			if (reaccionActual == reaccionDeProducto) {
				return true;
			}
		}
		return false;
	}

	// Verifica los tipos de alcohol cuando se le da click al boton
	public void verificaProductosDeReaccion () {

		modalPanel.SetActive (true);

		if (quedanProductos (productosPool.transform)) {
			//Enseña modal de que hay error en el pool de productos
			Debug.Log ("Te falta poner productos!");
			textoModal.text = "Te falta poner productos!";
			imagenModal.sprite = errorSprite;
		} else if (!checaProductos (productosDrop.transform)) {
			//Enseña modal de que hay error en el dropzone de productos
			Debug.Log ("Pusiste productos incorrectos!");
			textoModal.text = "Pusiste productos incorrectos!";
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

		int productosQuedan = 0;

		if (destruir) {
			foreach (Transform child in productosPool.transform) {
				GameObject.Destroy (child.gameObject);
			}
			foreach (Transform child in productosDrop.transform) {
				GameObject.Destroy (child.gameObject);
			}

			for (int i = 0; i < 25; i++) {
				if (!productosCreados [i])
					productosQuedan++;
			}

			if (productosQuedan < 8) {
				for (int i = 0; i < 25; i++)
					productosCreados [i] = false;
			}

			creaProductos (8);
			creaReaccion ();

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
