# Filmų recenzijų sistema

Šiame projekte reikia įgyvendinti **Filmų recenzijų sistemą**, kuri leistų vartotojams naršyti filmų žanrus, filmus, jų recenzijas bei aktorius.  
Sistema turi būti realizuota taikant **REST principus**, naudojant **.NET C# (API)** ir **React (GUI)**.  
Autentifikacija ir autorizacija vykdoma per **JWT** su „Access + Refresh Token“ strategija.  
Duomenys saugomi relacinėje **duomenų bazėje (PostgreSQL / MS SQL)**, o pats sprendimas talpinamas **debesų infrastruktūroje**.  

Projekte privaloma sukurti:
- Bent **5 taikomosios srities objektus** (Žanras, Filmas, Recenzija, Aktorius, Vartotojas)  
- Bent **15 API metodų** (CRUD ir hierarchiniai)  
- Bent **3 naudotojų roles** (Svečias, Narys, Administratorius)  
- Dokumentaciją su **OpenAPI (Swagger)** specifikacija bei naudojimo pavyzdžiais.  

## Taikomosios srities objektai
- **Filmo žanras**
- **Filmas**
- **Recenzija (Review)**
- **Aktorius**
- **Vartotojas (User)**

### Hierarchiniai ir prasminiai ryšiai
- **Filmo žanras → Filmas → Recenzija**
- **Filmas ↔ Aktorius** (daugelio prie daugelį ryšys)
- **Vartotojas → Recenzija** (recenzijos autorius)

---

## API metodai (REST, JSON, JWT)

### 1.  Filmo žanras
- `POST   /genres`         – sukurti žanrą
- `GET    /genres/{id}`    – gauti žanrą pagal ID
- `PUT    /genres/{id}`    – atnaujinti žanrą
- `DELETE /genres/{id}`    – ištrinti žanrą
- `GET    /genres`         – gauti visų žanrų sąrašą

### 2. Filmas
- `POST   /movies`             – sukurti filmą
- `GET    /movies/{id}`        – gauti filmą pagal ID
- `PUT    /movies/{id}`        – atnaujinti filmą
- `DELETE /movies/{id}`        – ištrinti filmą
- `GET    /movies/{id}/genres` – gauti visus žanro filmus (hierarchinis metodas)

### 3. Recenzija
- `POST   /reviews`             – sukurti recenziją
- `GET    /reviews/{id}`        – gauti recenziją pagal ID
- `PUT    /reviews/{id}`        – atnaujinti recenziją
- `DELETE /reviews/{id}`        – ištrinti recenziją
- `GET    /movies/{id}/reviews` – gauti visas konkretaus filmo recenzijas (hierarchinis metodas)

### 4. Aktorius
- `POST   /actors`             – sukurti aktorių
- `GET    /actors/{id}`        – gauti aktorių pagal ID
- `PUT    /actors/{id}`        – atnaujinti aktoriaus duomenis
- `DELETE /actors/{id}`        – ištrinti aktorių
- `GET    /movies/{id}/actors` – gauti konkretaus filmo aktorius (hierarchinis metodas)


### 5. Vartotojas
- `POST   /users`        – sukurti vartotoją
- `GET    /users/{id}`   – gauti vartotoją pagal ID
- `PUT    /users/{id}`   – atnaujinti vartotojo duomenis
- `DELETE /users/{id}`   – ištrinti vartotoją
- `GET    /users`        – gauti vartotojų sąrašą

## Nefunkciniai reikalavimai

- Duomenų bazė: relacinė (PostgreSQL / MS SQL)
- API: C#, RESTful, dokumentuota su OpenAPI (Swagger)
- Autentifikacija: JWT su Access + Refresh Token strategija
- GUI: React + Mantine UI, lentelės atvaizduojamos su MantineReactTable
- Debesų infrastruktūra: TBD
