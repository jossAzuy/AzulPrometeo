# AzulPrometeo
This is a collaborative repository for the AzulPrometeo Game Project.
The current version of the proyect is: 0.1

List of scripts made by each team member:

**RAZIEL**:
- **_FeedbackManager.cs_**: Muestra retroalimentación visual, como texto flotante, en respuesta a eventos del juego.
- **_FloatingFeedbackImage.cs_**: Muestra imágenes flotantes como retroalimentación visual.
- **_FloatingFeedbackText.cs_**: Muestra texto flotante como retroalimentación visual.
- **_RhythmArrowMover.cs_**: Controla el movimiento de flechas/barras en el sistema de ritmo.
- **_RhythmSystem.cs_**: Gestiona el sistema de ritmo, incluyendo la sincronización con el BPM.
- **_SoundManager.cs_**: Gestiona los efectos de sonido y la música del juego.
- **_Crosshair.cs_**: Muestra y mueve una mira en la pantalla basada en la posición del mouse.

**SAUL**:
- **_Bullet.cs_**: Controla el comportamiento de las balas disparadas por el jugador, como su movimiento y colisiones.
- **_PlayerCoins.cs_**: Lleva el conteo de las monedas recolectadas por el jugador.
- **_CoinPickup.cs_**: Gestiona la recolección de monedas por parte del jugador.
- **_EnemyAI.cs_**: Controla el comportamiento de los enemigos, incluyendo patrullaje, ataques y habilidades especiales.
- **_EnemySpawner.cs_**: Genera enemigos en puntos específicos y gestiona su cantidad máxima.
- **_EnemyDespawnNotifier.cs_**: Notifica al spawner cuando un enemigo es destruido para mantener el conteo de enemigos activos.
- **_EnemyBullet.cs_**: Gestiona las balas disparadas por los enemigos, incluyendo daño al jugador.

**DANIEL**:
- **_GameController.cs_**: Gestiona la lógica principal del juego, como el sistema de ritmo y el estado general.
- **_PlayerController.cs_**: Controla el movimiento y las acciones del jugador.
- **_PlayerHealth.cs_**: Gestiona la salud del jugador y el daño recibido.
- **_CameraRhythmEffects.cs_**: Aplica efectos visuales a la cámara, como sacudidas o cambios de post-procesamiento, sincronizados con el ritmo.
- **_AmmoDisplayManager.cs_**: Gestiona la visualización de la munición en la interfaz de usuario, incluyendo sprites y texto.
- **_PlayerAim.cs_**: Gestiona la dirección en la que el jugador apunta.
- **_GameOverManager.cs_**: Controla la lógica de la pantalla de "Game Over".
