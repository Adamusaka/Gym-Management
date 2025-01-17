# Gym-Management
Projektna za predmet IS 2025

Ta repozitorij vsebuje **Gym-Management**, ki je sestavljen iz treh glavnih delov: 
**C# .NET strežnik (backend)** in **Blazor odjemalec (frontend)**, oba poganja **MS SQL (SQL server)** podatkovna baza. Vse komponente delajo na svojemu containerju znotraj Dockerja.

## Kazalo

- [Pregled](#pregled)
- [Funkcionalnosti](#funkcionalnosti)
- [Tehnologije](#tehnologije)
- [Struktura Projekta](#struktura-projekta)
- [Uporaba](#uporaba)
- [Slike](#slike)

---

## Pregled

Sistem omogoča članom fitnesa prijavo in odjavo preko Blazor spletne aplikacije, administratorjem pa dodajanje naročnin ter pregled vseh uporabnikov.
Glavne komponente so:

- **Strežnik (C# .NET)**, ki skrbi za poslovno logiko, dostop do podatkov in API-je.
- **Blazor (frontend)** za uporabniški vmesnik.
- **MS SQL baza** za shranjevanje podatkov.

Zakaj Blazor?
Hotela sva se naučiti kakšne nove tehnologije in sem po malo brskanja na spletu se odločil za uporabo Blazor.

Kako vzpostaviti Docker containerje?

### Front-end:
docker build -t gym-management-app:latest .

docker tag gym-management-app:latest localhost:5000/gym-management-app:latest

docker run -d -p 30002:8001 --name gym-management-app --network gym-management-net --restart unless-stopped gym-management-app:latest

### Back end

docker volume create gym-management-vol
docker network create gym-management-net

docker build -t gym-management-api:latest .

docker run -d -p 30001:8000 --name gym-management-api --network gym-management-net --restart unless-stopped gym-management-api:latest

### Podatkovna baza:

docker run -p 30000:1433 --name gym-management-db --network gym-management-net -d -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=**Vnesi geslo**" -v mixeditor-vol:/var/opt/mssql --restart unless-stopped mcr.microsoft.com/mssql/server:2022-latest

---

## Funkcionalnosti

1. **Avtorizacija in Avtentikacija**  
   - Varnostno prijavljanje in registracija.
   - Dostop na podlagi vlog (navaden uporabnik "Customer" ali skrbnik "Admin").

2. **Prijava/Odjava v Fitnes**  
   - Uporabniki lahko ob prihodu v fitnes opravijo prijavo in ob odhodu odjavo.
   - Sistem hrani zgodovino prijav/odjav.

3. **Administratorski Vmesnik**
   - Pregled vseh uporabnikov.
   - Dodajanje naročnin za vsakega uporabnika.

---

## Tehnologije

- **.NET / C#** za strežniške storitve.
- **Blazor** za front-end razvoj.
- **MS SQL** za shranjevanje podatkov.
- **Docker** za zagon podatkovne baze in aplikacije.
- **Postman** za testiranje API.

---

## Struktura Projekta

### Backend

├─ Entities/<br/>
│ └─ Razredi podatkovnih modelov<br/>
├─ Models/<br/>
│ └─ Custom objekti podatkovih modelov<br/>
├─ Interfaces/<br/>
│ └─ Vmesniki (kontrakti), ki jih uporabljajo plasti BLL/DAL/Services<br/>
├─ Layers/<br/>
│ ├─ BLL/ (Business Logic Layer)<br/>
│ │ └─ Poslovna logika in obdelava podatkov<br/>
│ ├─ DAL/ (Data Access Layer)<br/>
│ │ └─ Pristop do podatkov in komunikacija z bazo<br/>
│ ├─ PL/ (Presentation Layer)<br/>
│ │ └─ API krmilniki, kjer se obdelujejo HTTP prošnje<br/>
│ └─ VL/ (Validation Layer)<br/>
│ │ └─ Validacija za vse vhodne podatke HTTP prošenj<br/>
└─ Services/<br/>
└─ Različne storitve (npr. DBservice), ki dopolnjujejo glavno poslovno logiko

### Frontend (Blazor)

├─ Components/<br/>
│ ├─ Layout/<br/>
│ │ ├─ Alerts/<br/>
│ │ │ ├─ ErrorAlert<br/>
│ │ │ └─ SuccessAlert<br/>
│ │ └─ Table<br/>
│ └─ Forms/<br/>
│ ├─ SigninForm<br/>
│ └─ SignupForm<br/>
├─ Pages/<br/>
│ ├─ Index<br/>
│ ├─ Home<br/>
│ ├─ Signin<br/>
│ └─ Signup<br/>
│─ wwwroot/<br/>
└ └─ Ikona za spletno okno

---

## Uporaba

### Registracija / Prijava

Odprite Blazor spletno aplikacijo.
Ustvarite nov račun ali se prijavite z obstoječimi podatki.

Prijava / Odjava v Fitnes
Po prijavi lahko uporabnik z enim klikom označi prihod in odhod iz fitnesa.

### Administratorske Funkcije

Kot skrbnik se prijavite in v skrbniškem delu vidite vse uporabnike.
Dodajate naročnine posameznim članom.

---

## Slike

### Sign in form
![image](https://github.com/user-attachments/assets/0eff719e-a22b-4733-86e7-03028287b384)

### Sign up form
![image](https://github.com/user-attachments/assets/41869abb-96b0-488a-8645-9eed80a2d239)
