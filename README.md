El problema del barbero dormilón es un problema planteado por Dijkstra en 1971, un ejemplo de uso de semáforos para implementar la concurrencia.
El problema consiste en realizar la actividad del barbero sin que ocurran condiciones de carrera, siendo la solución el uso de semáforos y objetos de exclusión mutua para proteger la sección crítica. Este ejemplo es instructivo, porque los problemas encontrados cuando se intenta proporcionar un acceso adaptado a los recursos de la barbería, es similar a aquéllos encontrados en un sistema operativo real.
Una barbería tiene un barbero y un área de espera que puede acomodar a cuatro clientes en un sofá. Un cliente no entrará en la barbería si está llena. Una vez dentro, el cliente toma asiento en el sofá. Cuando el barbero está libre, sirve al cliente que ha permanecido en el sofá durante un tiempo mayor. En dado caso que no haya clientes, el barbero se pone a dormir.
Un semáforo es una variable protegida (o tipo abstracto de datos) que constituye el método clásico para restringir o permitir el acceso a recursos compartidos (por ejemplo, un recurso de almacenamiento) en un entorno de multiprocesamiento. Inventados por Dijkstra y se usados por primera vez en el sistema operativo THEOS.

Para el desarrollo de este programa se ha usado el lenguaje de c# con ayuda de la aplicación de Windows forms para implementar una interfaz gráfica.

En resumen, lo que se hace dentro del programa es que desde el momento en que inicia el problema, se genera un temporizador con tiempo aleatorio entre 0 a 7 segundos para que pueda aparecer un nuevo cliente. Una vez que llega un cliente se verifica si está en un estado de “cortando” o no, y si hay más clientes o si es el primero, lo cual hace que se ejecute la acción de cortar el pelo a ese cliente y generando al mismo tiempo otro temporizador para que pueda terminar de cortar el pelo a ese cliente, durando entre 5 a 15 segundos de manera aleatoria. Siempre cortará el pelo al cliente que haya esperado más, aunque la forma e la que se siente será el primer lugar que encuentre. 



Bibliografía
Desconocido. (7 de 5 de 2016). Algoritmo barbero. Obtenido de Problema del Barbero durmiente: https://algoritmobarbero.blogspot.com/2016/05/problema-del-barbero-durmiente-en.html
Stallings, W. (2005). Sistemas operativos. Aspectos internos y principios de diseño. Madrid: Pearson.


