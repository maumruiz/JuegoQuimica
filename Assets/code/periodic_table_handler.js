
var preguntas = Array(); 
var actual;

function Start () {
	loadQuestions();
	nextQuestion();
}

function Update () {

}

function loadQuestions(){
preguntas.push({
	'p':'Presiona H', 
	'r':'H'
	});

preguntas.push({
	'p':'Presiona He', 
	'r':'He'
	});

preguntas.push({
	'p':'Presiona Li', 
	'r':'Li'
	});

preguntas.push({
	'p':'Presiona Ce', 
	'r':'Ce'
	});

preguntas.push({
	'p':'Presiona Db', 
	'r':'Db'
	});

preguntas.push({
	'p':'Presiona Al', 
	'r':'Al'
	});

preguntas.push({
	'p':'Presiona Mn', 
	'r':'Mn'
	});

preguntas.push({
	'p':'Presiona Ag', 
	'r':'Ag'
	});

preguntas.push({
	'p':'Presiona Au', 
	'r':'Au'
	});

preguntas.push({
	'p':'Presiona Cd', 
	'r':'Cd'
	});

}

function nextQuestion(){
	actual = Random.Range(0,preguntas.length-1);
	showQuestion();
}

function showQuestion(){
	Debug.Log(preguntas[actual]['p']);
}

function verifyAnswer(answer : String){
	return (preguntas[actual]['r'] == answer);
}

function ButtonPressed(element : String){
	if (verifyAnswer(element)){
		Debug.Log("Respuesta Correcta");
	}
	else {
		Debug.Log("Respuesta Incorrecta");
	}
	nextQuestion();
}





 