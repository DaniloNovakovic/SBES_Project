# SBES_Project

Implementirati servis za replikaciju podataka od primarnog (primary) na sekundarni (backup) sajt.

Klijent šalje podatke (tipa Alarm) servisu na primarnom sajtu.

Klasa Alarm sadrži:

- vreme generisanja alarma,
- ime klijenta koji je izgenerisao podatke,
- poruku - definisati standardne poruke u okviru resursnog fajla,
- rizik - koji se računa u momentu kad se alarm izgeneriše, npr. metoda CalculateRisk

Postoje tri tipa klijenata:

- Reader klijenti koji mogu da dobiju informacije o trenutnom stanju u bazi podataka
- AlarmGenerator klijenti imaju pravo da generišu nove alarme.
- AlarmAdmin klijenti koji imaju pravo brisanja svih alarma iz baze podataka. Dodatno, ovi klijenti mogu prihvatati zahteve za brisanje od svih ostalih klijenata - u tom slučaju brišu alarme generisanje od strane klijenta koji je poslao zahtev (na osnovu imena klijenta, koje se ne prosleđuje, već metoda za brisanje sama zaključuje).

Primarni servis smešta izgenerisane alarme u tekstualnu datoteku samo ako su stigli od ovlašćenog klijenta. Dodatno, primarni servis smešta podatke u odgovarajući buffer za potrebe repliciranja. Klijent i servis na primarnom sajtu komuniciraju preko Windows autentifikacionog protokola, a autorizacija je zasnovana na RBAC modelu.

Sekundarni sajt se sastoji od sekundarnog servisa koji prima sve podatke pristigle replikacijom, smešta ih u tekstualnu datoteku, i ispisuje na konzolu.
