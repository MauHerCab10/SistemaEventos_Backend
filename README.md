ng version: mirar la version de Angular y de Node
alt + shift + f: darle formato al codigo


CREACIóN DEL PROYECTO:
1. creo la carpeta del proyecto
2. comando 'cmd' en la ruta de la carpeta
3. comando 'code .'
4. comando 'ng new Nombre_Proyecto' en VS Code
5. comando 'cd Nombre_Proyecto'
6. comando 'npm install --force'
7. comando 'ng serve -o'



borrar carpeta 'node_modules' y archivo 'package-lock.json'
npm install
ng add @angular/material

ng g service services/Acceso
ng g service services/Producto

ng g component pages/Login --skip-tests
ng g component pages/Registro --skip-tests
ng g component pages/Inicio --skip-tests

ng g interceptor seguridad/autenticacion
ng g guard seguridad/autenticacion --implements CanActivate
