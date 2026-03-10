# 3D Block Puzzle Game

[🇺🇸 English](#english) | [🇹🇷 Türkçe](#türkçe)

---

<a name="english"></a>
## English

This project is a modular 3D block puzzle game developed with modern Unity standards and **Zenject (Dependency Injection)** principles. The primary focus of the project is to implement a sustainable code structure and follow **"Screaming Architecture"** patterns.

### 🏗 Architectural Overview

The project is built using a **Feature-based architecture**, moving away from traditional "Manager-heavy" structures. This ensures that every system (Board, Blocks, UI, etc.) has its own decoupled area of responsibility.

#### Technologies & Principles

* **Zenject (DI):** Used for managing dependencies between classes and ensuring loose coupling.
* **Screaming Architecture:** Folder structure is organized to reflect the game's features directly, making the intent of the system clear at a glance.
* **Data-Driven Design:** Level data and block shapes are managed via **ScriptableObjects**.
* **Object Pooling:** Implemented for performance optimization during block generation and destruction.

### 📂 Project Organization

All development assets are centralized under the `_Project` root folder to maintain a clean workspace:

* **Scripts:** Core logic and feature-specific systems.
* **Data:** Level designs and shape definitions (ScriptableObjects).
* **Prefabs:** Modular game objects and Unity UI components.
* **Art:** 3D models and visual assets.

### 🎮 Scene & Hierarchy Management

The game scene follows a categorized hierarchy for better workflow and debugging:

* **ENVIRONMENT:** Camera, lighting, and visual background.
* **SYSTEM:** Zenject Context, global managers, and installers.
* **GAMEPLAY:** Active game elements (Grid, Shapes, InputHandler).
* **UI:** User interface elements and EventSystem.

---

<a name="türkçe"></a>
## Türkçe

Bu proje, modern Unity geliştirme standartları ve **Zenject (Dependency Injection)** prensipleri kullanılarak geliştirilmiş, modüler bir 3D blok bulmaca oyunudur. Projenin temel odağı, sürdürülebilir bir kod yapısı ve **"Screaming Architecture"** (Bağıran Mimari) prensiplerini uygulamaktır.

### 🏗 Mimari Yapı

Proje, geleneksel "Manager" odaklı yapıdan arındırılarak **Özellik Bazlı (Feature-based)** bir mimari ile inşa edilmiştir. Bu sayede her sistem (Board, Blocks, UI, vb.) kendi sorumluluk alanına sahiptir.

#### Kullanılan Teknolojiler & Prensipler

* **Zenject (DI):** Sınıflar arası bağımlılıkları yönetmek ve esnek bir yapı kurmak için kullanılmıştır.
* **Screaming Architecture:** Klasör yapısı, oyunun özelliklerini ve işlevlerini doğrudan anlatacak şekilde düzenlenmiştir.
* **Data-Driven Design:** Seviye ve şekil verileri **ScriptableObject** ile yönetilmektedir.
* **Object Pooling:** Performans optimizasyonu için nesne havuzlama sistemi kullanılmıştır.

### 📂 Proje Organizasyonu

Tüm geliştirme varlıkları, temiz bir çalışma alanı için `_Project` ana klasörü altında toplanmıştır:

* **Scripts:** Çekirdek mantık ve sisteme özel özellikler.
* **Data:** Seviye tasarımları ve şekil tanımlamaları.
* **Prefabs:** Modüler oyun objeleri ve UI bileşenleri.
* **Art:** 3D modeller ve görsel varlıklar.

### 🎮 Sahne ve Hiyerarşi Yönetimi

Oyun sahnesi, iş akışını ve hata ayıklamayı kolaylaştırmak için kategorize edilmiş bir hiyerarşi yapısına sahiptir:

* **ENVIRONMENT:** Kamera, ışıklandırma ve görsel arka plan.
* **SYSTEM:** Zenject Context, genel yöneticiler ve yükleyiciler.
* **GAMEPLAY:** Aktif oyun elemanları (Grid, Shapes, InputHandler).
* **UI:** Kullanıcı arayüzü elemanları ve EventSystem.