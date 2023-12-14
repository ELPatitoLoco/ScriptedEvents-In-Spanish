![VERSION](https://img.shields.io/github/v/release/Thundermaker300/ScriptedEvents?include_prereleases&style=for-the-badge)
![DOWNLOADS](https://img.shields.io/github/downloads/Thundermaker300/ScriptedEvents/total?style=for-the-badge)
[![DISCORD](https://img.shields.io/discord/1060274824330620979?label=Discord&style=for-the-badge)](https://discord.gg/3j54zBnbbD)


# ScriptedEvents
SCP:SL Exiled plugin to create event "scripts". These scripts can be set up to run once per round, multiple times per round, or by command only.

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
This method works by adding a reference to the plugin.
Este metodo funciona añadiendo una referencia al plugin.

Crea una nueva clase, necesita heredar `ScriptedEvents.API.Actions.IAction`, luego implementar esta interfaz. 

Entonces, en tu OnEnabled añade `ScriptedEvents.API.ApiHelper.RegisterActions();`

El problema de usar este método es que su plugin SÓLO funcionará si ScriptedEvents también está instalado, lo que no es ideal ya que puede que un servidor use tu plugin pero no ScriptedEvents.

### Reflection
The alternative to the above method is by using reflection to access the `ApiHelper` class. From there, call the `RegisterCustomAction(string, Func<string[], Tuple<bool, string>>)` method.

The above method takes a string, the name of the plugin, and it takes a defined function. This function gives you a `string[]`, representing the arguments that were given from the script. It must return a `Tuple<bool, string>`, with the bool representing whether or not execution was successful, and the message to show. If it is NOT successful, a message should be provided. If it is successful, a message is optional (should be set to `string.Empty`).

If your plugin is disabled in-game, and Scripted Events is still running, this may cause a problem if a script uses your action. As such, it is recommended to call the `ApiHelper.UnregisterCustomAction(string name)` method if your action is no longer usable.

For ease of debugging, both `RegisterCustomAction` and `UnregisterCustomAction` return a string message representing whether or not they were successful.

This method is much more recommended, as your plugin does not need to have Scripted Events installed in order for your plugin to function. However, it is not as straight forward as the previous method, and reflection is significantly slower than the previous method (which is why you only need to use it once in your plugin).

To view an example of this method in action, see the [Round Reports](https://github.com/Thundermaker300/RoundReports/blob/master/RoundReports/ScriptedEventsIntegration.cs) implementation of it.

#### Other Reflection API
* `IEnumerable<Player> ApiHelper.GetPlayers(string input, int max = -1)` - Gets a list of players based on input variables, and a maximum amount to select a random maximum. This should be used instead of the classic `Player.Get()` as this method also supports all of Scripted Events' variables (including user-defined variables).
* `Tuple<bool, float> ApiHelper.Math(string input)` - Performs a math calculation using the given string. This method supports all of Scripted Events' variables (including user-defined variables). Returns a success boolean and the result of the equation as a float.
