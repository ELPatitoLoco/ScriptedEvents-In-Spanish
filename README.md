![VERSION](https://img.shields.io/github/v/release/Thundermaker300/ScriptedEvents?include_prereleases&style=for-the-badge)
![DOWNLOADS](https://img.shields.io/github/downloads/Thundermaker300/ScriptedEvents/total?style=for-the-badge)
[![DISCORD](https://img.shields.io/discord/1060274824330620979?label=Discord&style=for-the-badge)](https://discord.gg/3j54zBnbbD)


### ADVERTENCIA 
Este repositorio ha sido creado para aquellas personas que quieran usar el plugin y sepan poco o nada de Ingles, si sabes ingles te recomiendo enormemente usar el [repositorio original](https://github.com/Thundermaker300/ScriptedEvents), he traducido todo como he podido pero obviamente no es perfecto y si de verdad quieres programar te recomiendo aprender ingles, todo dicho si necesitas ayuda pingeame en discord o enviame un mensaje(el_patito).

# ScriptedEvents
Un plugin de SCP:SL hecho con Exiled para crear eventos "scripts". Los scripts pueden ser hechos para ser usados una vez por ronda, multiples o por solo con commandos.

## Getting Started
Aviso: Este plugin es muy complejo y tiene un montón de características. Sin embargo, una vez que lo entiendes, las capacidades son casi infinitas. Mi mejor sugerencia es jugar y probar cosas con el plugin, ya que es la forma más fácil de aprenderlo. Algunos consejos para empezar:
* Lee los documentos que se generan cuando instalas el plugin por primera vez y reinicias el servidor. La consola te dirá dónde se encuentran la primera vez (normalmente directamente dentro de la carpeta de Configuracion(Ejemplo: EXILED/Configs)).
* El comando `shelp` de la consola del servidor te va a ayudar mucho. Este comando generará documentación y la abrirá en un archivo `.txt`. Escribe `shelp LIST` en tu consola de servidor para generar una lista de acciones.
  * Nota para los usuarios de Pterodactyl: A Pterodactyl no le gusta abrir archivos bajo demanda y generalmente arrojará un error de permiso. Aún así generará el archivo dentro de su carpeta `Configs/ScriptedEvents`, sólo que no lo abrirá. Como tal, se recomienda utilizar un servidor local para el uso de este comando.
* El comando padre de remote-admin para este plugin es `scriptedevents` (alias: `script`, `scr`). Ejecutar este comando mostrará ejemplos de cómo usarlo.

### Permisos
* `script.action` - Ejecutar una única acción no lógica mediante comando.
* `script.execute` - Ejecutar un script.
* `script.list` - Ver todos los scripts.
* `script.read` - Leer un script.
* `script.stopall` - Detener todos los scripts en ejecución.

### Configuracion por defecto
```yml
scripted_events:
# Whether or not to enable the Scripted Events plugin.
  is_enabled: true
  debug: false
  # Enable logs for starting/stopping scripts.
  enable_logs: true
  # If a script encounters an error, broadcast a notice to the person who ran the command, informing of the error. The broadcast ONLY shows to the command executor.
  broadcast_issues: true
  # If set to true, players with overwatch enabled will not be affected by any commands related to players.
  ignore_overwatch: true
  # List of scripts to run as soon as the round starts.
  auto_run_scripts: []
  # List of scripts to automatically re-run as soon as they finish.
  loop_scripts: []
  # The string to use for countdowns.
  countdown_string: '<size=26><color=#5EB3FF><b>{TEXT}</b></color></size>\n{TIME}'
  # The maximum amount of actions that can run in one second, before the script is force-stopped. Increasing this value allows for more actions to occur at the same time, but increases the risk of the server crashing (or restarting due to missed heartbeats). This maximum can be bypassed entirely by including the "!-- NOSAFETY" flag in a script.
  max_actions_per_second: 25
  # Define a custom set of permissions used to run a certain script. The provided permission will be added AFTER script.execute (eg. script.execute.examplepermission for the provided example).
  required_permissions:
    ExampleScriptNameHere: examplepermission
  # [ADVANCED] Define scripts to execute when certain events occur.
  on: {}
```

## Para desarolladores
Hay dos metodos para añadir tus propias acciones a ScriptedEvents en tu plugin.

### Plugin de referencia directa
Este metodo funciona añadiendo una referencia al plugin.

Crea una nueva clase, necesita heredar `ScriptedEvents.API.Actions.IAction`, luego implementar esta interfaz. 

Entonces, en tu OnEnabled añade `ScriptedEvents.API.ApiHelper.RegisterActions();`

El problema de usar este método es que su plugin SÓLO funcionará si ScriptedEvents también está instalado, lo que no es ideal ya que puede que un servidor use tu plugin pero no ScriptedEvents.

### Refleccion
La alternativa al método anterior es utilizar reflection para acceder a la `ApiHelper`. Desde allí, llama al método `RegisterCustomAction(string, Func<string[], Tuple<bool, string><>)`.

El método anterior toma una cadena, el nombre del plugin, y toma una función definida. Esta función te da una `cadena[]`, que representa los argumentos que fueron dados desde el script. Debe devolver una `Tuple<bool, string>`, con el bool representando si la ejecución fue exitosa o no, y el mensaje a mostrar. Si NO tiene éxito, se debe proporcionar un mensaje. Si tiene éxito, el mensaje es opcional (debería ser `string.Empty`).

Si tu plugin está deshabilitado en el juego, y Scripted Events sigue ejecutándose, esto puede causar un problema si un script utiliza tu acción. Por ello, se recomienda llamar al método `ApiHelper.UnregisterCustomAction(string name)` si tu acción ya no es utilizable.

Para facilitar el "debugging", tanto `RegisterCustomAction` como `UnregisterCustomAction` devuelven una cadena de texto(string) que representa si tuvieron éxito o no.

Este método es mucho más recomendable, ya que tu plugin no necesita tener Scripted Events instalado para que tu plugin funcione. Sin embargo, no es tan sencillo como el método anterior, y reflección es significativamente más lenta que el método anterior (razón por la cual sólo necesitas usarlo una vez en tu plugin).

Para ver un ejemplo de este método en acción, consulte la implementación del mismo en [Informes de ronda(Round Reports)](https://github.com/Thundermaker300/RoundReports/blob/master/RoundReports/ScriptedEventsIntegration.cs).

#### Otra Api de Refleccion
* `IEnumerable<Player> ApiHelper.GetPlayers(string input, int max = -1)` - Obtiene una lista de jugadores basada en las variables de entrada, y una cantidad máxima para seleccionar un máximo aleatorio. Esto debería usarse en lugar del clásico `Player.Get()` ya que este método también soporta todas las variables de Scripted Events (incluyendo variables definidas por el usuario).
* `Tuple<bool, float> ApiHelper.Math(string input)` - Realiza un cálculo matemático utilizando la cadena dada. Este método admite todas las variables de Scripted Events (incluidas las variables definidas por el usuario). Devuelve un booleano de éxito y el resultado de la ecuación como un float.

### Creditos
Plugin original hecho por **ThunderMaker300** <br/>
Traducido por el_patito_loco
