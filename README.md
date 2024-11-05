<h1>MagicVilla API 🏠</h1>

<p><strong>MagicVilla API</strong> es una API RESTful construida con <strong>.NET 8</strong> para la gestión de un catálogo de villas, diseñada para soportar operaciones CRUD y facilitar la integración con un cliente MVC. Esta aplicación web es ideal para gestionar villas y su información relevante, con un enfoque en seguridad, eficiencia y escalabilidad.</p>

<p><a href="http://www.magicvilla-api.somee.com" target="_blank">Despliegue en vivo de la API</a> 🌐</p>

<hr>

<h2>🚀 Descripción del Proyecto</h2>

<p>MagicVilla API proporciona una arquitectura robusta para manejar villas, ofreciendo endpoints que permiten crear, leer, actualizar y eliminar registros de villas. Esta API REST está desplegada en un entorno en línea, permitiendo su consumo en tiempo real. La solución también integra autenticación segura y una interfaz de documentación para su exploración y prueba.</p>

<hr>

<h2>🛠 Tecnologías Utilizadas</h2>

<ul>
    <li><strong>ASP.NET Core</strong> - Framework base para la aplicación web</li>
    <li><strong>.NET 8</strong> - Versión de .NET para mejorar rendimiento y compatibilidad</li>
    <li><strong>Entity Framework Core</strong> - ORM para operaciones CRUD y mapeo de datos</li>
    <li><strong>SQL Server 2022</strong> - Base de datos para almacenamiento de datos de villas</li>
    <li><strong>.NET Identity</strong> - Proporciona autenticación y autorización segura</li>
    <li><strong>Swagger</strong> - Documentación interactiva de la API</li>
    <li><strong>AutoMapper</strong> - Manejo de DTOs para una gestión eficiente de datos</li>
    <li><strong>GitHub</strong> - Control de versiones y colaboración</li>
</ul>

<hr>

<h2>📄 Características de la API</h2>

<ol>
    <li><strong>Operaciones CRUD</strong>: Endpoints para crear, leer, actualizar y eliminar villas.</li>
    <li><strong>DTOs y AutoMapper</strong>: Estructuras de datos optimizadas para el manejo de información.</li>
    <li><strong>Autenticación Segura</strong>: Implementación de .NET Identity para el acceso seguro.</li>
    <li><strong>Paginación y Versionado</strong>: Optimización para mejorar la escalabilidad y la experiencia del usuario.</li>
    <li><strong>Documentación con Swagger</strong>: Interfaz interactiva para explorar y probar la API.</li>
</ol>

<hr>

<h2>⚙️ Requisitos Previos</h2>

<ul>
    <li><strong>.NET 8 SDK</strong> instalado</li>
    <li><strong>SQL Server 2022</strong> o superior</li>
    <li><strong>Visual Studio</strong> o <strong>VS Code</strong> (para desarrollo)</li>
</ul>

<hr>

<h2>📝 Instalación y Configuración</h2>

<ol>
    <li><strong>Clona el repositorio:</strong>
        <pre><code>git clone https://github.com/tu-usuario/magicvilla-api.git
cd magicvilla-api</code></pre>
    </li>
    <li><strong>Configura la cadena de conexión en <code>appsettings.json</code>:</strong> Asegúrate de que el archivo <code>appsettings.json</code> esté configurado correctamente con la cadena de conexión a tu SQL Server.</li>
    <li><strong>Aplica las migraciones de Entity Framework Core:</strong>
        <pre><code>dotnet ef database update</code></pre>
    </li>
    <li><strong>Inicia el proyecto:</strong>
        <pre><code>dotnet run</code></pre>
    </li>
</ol>

<hr>

<h2>🌐 Endpoints Principales</h2>

<table>
    <thead>
        <tr>
            <th>Método</th>
            <th>Endpoint</th>
            <th>Descripción</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>GET</td>
            <td>/api/villas</td>
            <td>Obtiene la lista de todas las villas</td>
        </tr>
        <tr>
            <td>GET</td>
            <td>/api/villas/{id}</td>
            <td>Obtiene una villa por ID</td>
        </tr>
        <tr>
            <td>POST</td>
            <td>/api/villas</td>
            <td>Crea una nueva villa</td>
        </tr>
        <tr>
            <td>PUT</td>
            <td>/api/villas/{id}</td>
            <td>Actualiza una villa existente</td>
        </tr>
        <tr>
            <td>DELETE</td>
            <td>/api/villas/{id}</td>
            <td>Elimina una villa</td>
        </tr>
    </tbody>
</table>

<p>Para explorar todos los endpoints, visita la <a href="http://www.magicvilla-api.somee.com/swagger" target="_blank">documentación de Swagger</a>.</p>

<hr>

<h2>🔐 Seguridad</h2>

<p>MagicVilla API utiliza <strong>.NET Identity</strong> para proteger los endpoints que requieren autenticación. Los usuarios pueden registrarse e iniciar sesión para obtener acceso a ciertos recursos de la API.</p>

<hr>

<h2>📂 Estructura del Proyecto</h2>

<ul>
    <li><strong>Controllers</strong>: Controladores que manejan las solicitudes HTTP y gestionan las rutas de la API.</li>
    <li><strong>Models</strong>: Clases de datos que representan las entidades del proyecto.</li>
    <li><strong>Data</strong>: Configuración de Entity Framework Core y acceso a datos.</li>
    <li><strong>DTOs</strong>: Objetos de transferencia de datos utilizados para la interacción con la API.</li>
    <li><strong>Mapping</strong>: Configuración de AutoMapper para transformar entre entidades y DTOs.</li>
</ul>

<hr>

<h2>🛠 Despliegue</h2>

<p>La API está desplegada en: <a href="http://www.magicvilla-api.somee.com" target="_blank">http://www.magicvilla-api.somee.com</a></p>

<hr>

<h2>👥 Contribuciones</h2>

<p>¡Las contribuciones son bienvenidas! Si deseas contribuir, por favor sigue estos pasos:</p>

<ol>
    <li>Haz un fork del proyecto.</li>
    <li>Crea una nueva rama (<code>git checkout -b feature/nueva-funcionalidad</code>).</li>
    <li>Realiza tus cambios y haz commit (<code>git commit -m 'Agregar nueva funcionalidad'</code>).</li>
    <li>Haz push a la rama (<code>git push origin feature/nueva-funcionalidad</code>).</li>
    <li>Abre un Pull Request.</li>
</ol>

<hr>

<h2>📄 Licencia</h2>

<p>Este proyecto está bajo la Licencia MIT. Consulta el archivo <a href="LICENSE" target="_blank">LICENSE</a> para más detalles.</p>

<hr>

<h2>📬 Contacto</h2>

<p>Para preguntas o comentarios, puedes contactarme en:</p>

<ul>
    <li><strong>LinkedIn</strong>: <a href="https://www.linkedin.com/in/gabriel-m-gonzalez" target="_blank">Gabriel M. González</a></li>
    <li><strong>GitHub</strong>: <a href="https://github.com/tu-usuario" target="_blank">https://github.com/tu-usuario</a></li>
</ul>
