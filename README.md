# Lommeregner (.NET 8, WinForms + Konsol)

En lille C#-lommeregner med både **konsol-UI** og **Windows UI (WinForms)**. Projektet indeholder en `Calculator`-model, en menustyret konsol-app, og et WinForms-UI. Der er også support til **DocFX**-dokumentation som kan publiceres til **GitHub Pages**.

## Funktioner
- Basale operationer: plus, minus, gange, division, modulus
- Potensering og kvadratrod
- Procenter og fakultet
- Logaritmer (vilkårlig base) og naturlig logaritme
- Trigonometriske funktioner (grader): sin, cos, tan
- Konsol-UI _og_ WinForms-UI i samme projekt (`--ui` flag)

## Projektstruktur
Lommeregner/
├─ Program.cs
├─ Lommeregner.csproj
├─ src/
│ ├─ Model/
│ │ └─ Lommeregner.cs # Calculator
│ ├─ Konsol App/
│ │ └─ KonsolApp.cs # Konsol UI
│ └─ Windows GUI/
│ └─ MainGUI.cs # WinForms UI
├─ docfx.json # DocFX konfiguration (tilføjes)
├─ index.md # Forside til docs (tilføjes)
├─ api/
│ └─ index.md # API-forside (tilføjes)
└─ toc.yml # Docs TOC (tilføjes)


> **Bemærk:** Sørg for at din `.csproj` har `<GenerateDocumentationFile>true</GenerateDocumentationFile>` (det er det i denne skabelon).

## Krav
- .NET SDK 8.0+
- Windows (for WinForms-delen)
- (Valgfrit) DocFX installeret som dotnet tool

## Kørsel
**Konsol (standard):**
dotnet run

**Windows UI:**
dotnet run -- --ui

## Dokumentation (DocFX)

Denne repo er sat op til at generere dokumentation via DocFX og publicere den til GitHub Pages via GitHub Actions.

Lokalt build

Installer DocFX som dotnet tool (første gang):
```bash
dotnet tool install -g docfx
```
Byg metadata og site (fra repo-roden hvor docfx.json ligger):
```bash
docfx metadata docfx.json
docfx build docfx.json
```
Den genererede side ligger i mappen _site/ – åbn ./_site/index.html i din browser.

## CI/CD til GitHub Pages

Der ligger en workflow-fil i .github/workflows/docs.yml som:
bygger metadata og site med DocFX, og deployer til GitHub Pages.

Slå Pages til i repoets Settings → Pages og vælg Source: GitHub Actions. Efter et push til main, vil workflow’et køre og publicere dokumentationen. Den bliver typisk tilgængelig på:
```html
https://<dit-github-brugernavn>.github.io/<dit-repo-navn>/
```

**Workflow virker ikke endnu**
