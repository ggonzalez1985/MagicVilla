# MagicVilla
Proyecto Restful Web Api

MagicVilla API 🏠
MagicVilla API es una API RESTful construida con .NET 8 para la gestión de un catálogo de villas, diseñada para soportar operaciones CRUD y facilitar la integración con un cliente MVC. Esta aplicación web es ideal para gestionar villas y su información relevante, con un enfoque en seguridad, eficiencia y escalabilidad.

🚀 Descripción del Proyecto
MagicVilla API proporciona una arquitectura robusta para manejar villas, ofreciendo endpoints que permiten crear, leer, actualizar y eliminar registros de villas. Esta API REST está desplegada en un entorno en línea, permitiendo su consumo en tiempo real. La solución también integra autenticación segura y una interfaz de documentación para su exploración y prueba.

Despliegue en vivo de la API 🌐

🛠 Tecnologías Utilizadas
ASP.NET Core - Framework base para la aplicación web
.NET 8 - Versión de .NET para mejorar rendimiento y compatibilidad
Entity Framework Core - ORM para operaciones CRUD y mapeo de datos
SQL Server 2022 - Base de datos para almacenamiento de datos de villas
.NET Identity - Proporciona autenticación y autorización segura
Swagger - Documentación interactiva de la API
AutoMapper - Manejo de DTOs para una gestión eficiente de datos
GitHub - Control de versiones y colaboración
📄 Características de la API
Operaciones CRUD: Endpoints para crear, leer, actualizar y eliminar villas.
DTOs y AutoMapper: Estructuras de datos optimizadas para el manejo de información.
Autenticación Segura: Implementación de .NET Identity para el acceso seguro.
Paginación y Versionado: Optimización para mejorar la escalabilidad y la experiencia del usuario.
Documentación con Swagger: Interfaz interactiva para explorar y probar la API.
⚙️ Requisitos Previos
.NET 8 SDK instalado
SQL Server 2022 o superior
Visual Studio o VS Code (para desarrollo)
📝 Instalación y Configuración
Clona el repositorio:

bash
Copiar código
git clone https://github.com/tu-usuario/magicvilla-api.git
cd magicvilla-api
Configura la cadena de conexión en appsettings.json:

Asegúrate de que el archivo appsettings.json esté configurado correctamente con la cadena de conexión a tu SQL Server.

Aplica las migraciones de Entity Framework Core:

bash
Copiar código
dotnet ef database update
Inicia el proyecto:

bash
Copiar código
dotnet run
🌐 Endpoints Principales
Método	Endpoint	Descripción
GET	/api/villas	Obtiene la lista de todas las villas
GET	/api/villas/{id}	Obtiene una villa por ID
POST	/api/villas	Crea una nueva villa
PUT	/api/villas/{id}	Actualiza una villa existente
DELETE	/api/villas/{id}	Elimina una villa
Para explorar todos los endpoints, visita la documentación de Swagger.

🔐 Seguridad
MagicVilla API utiliza .NET Identity para proteger los endpoints que requieren autenticación. Los usuarios pueden registrarse e iniciar sesión para obtener acceso a ciertos recursos de la API.

📂 Estructura del Proyecto
Controllers: Controladores que manejan las solicitudes HTTP y gestionan las rutas de la API.
Models: Clases de datos que representan las entidades del proyecto.
Data: Configuración de Entity Framework Core y acceso a datos.
DTOs: Objetos de transferencia de datos utilizados para la interacción con la API.
Mapping: Configuración de AutoMapper para transformar entre entidades y DTOs.
🛠 Despliegue
La API está desplegada en: http://www.magicvilla-api.somee.com

👥 Contribuciones
¡Las contribuciones son bienvenidas! Si deseas contribuir, por favor sigue estos pasos:

Haz un fork del proyecto.
Crea una nueva rama (git checkout -b feature/nueva-funcionalidad).
Realiza tus cambios y haz commit (git commit -m 'Agregar nueva funcionalidad').
Haz push a la rama (git push origin feature/nueva-funcionalidad).
Abre un Pull Request.
📄 Licencia
Este proyecto está bajo la Licencia MIT. Consulta el archivo LICENSE para más detalles.

📬 Contacto
Para preguntas o comentarios, puedes contactarme en:

LinkedIn: Gabriel M. González
GitHub: https://github.com/tu-usuario
