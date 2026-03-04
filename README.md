# 3D Block Puzzle Game

Bu proje, modern Unity geliştirme standartları ve **Zenject (Dependency Injection)** prensipleri kullanılarak geliştirilmiş, modüler bir 3D blok bulmaca oyunudur. Projenin temel odağı, sürdürülebilir bir kod yapısı ve **"Screaming Architecture"** prensiplerini uygulamaktır.

## 🏗 Mimari Yapı
Proje, geleneksel "Manager" odaklı yapıdan arındırılarak **Feature-based (Özellik Bazlı)** bir mimari ile inşa edilmiştir. Bu sayede her sistem (Board, Blocks, UI, vb.) kendi sorumluluk alanına sahiptir.

### Kullanılan Teknolojiler & Prensipler
* **Zenject (DI):** Sınıflar arası bağımlılıkları yönetmek için kullanılmıştır.
* **Screaming Architecture:** Klasör yapısı oyun özelliklerini doğrudan anlatacak şekilde düzenlenmiştir.
* **Data-Driven Design:** Seviye ve şekil verileri ScriptableObject ile yönetilmektedir.
* **Object Pooling:** Performans optimizasyonu için pooling sistemi kullanılmıştır.

## 📂 Proje Organizasyonu
Tüm geliştirme varlıkları `_Project` ana klasörü altında toplanmıştır:

* **Scripts:** Core logic ve sistem özellikleri.
* **Data:** Seviye tasarımları ve şekil tanımlamaları.
* **Prefabs:** Modüler oyun objeleri ve UI bileşenleri.
* **Art:** 3D modeller ve görsel varlıklar.

## 🎮 Sahne ve Hiyerarşi Yönetimi
Oyun sahnesi kategorize edilmiş bir hiyerarşi yapısına sahiptir:

* **ENVIRONMENT:** Kamera, ışık ve görsel zemin.
* **SYSTEM:** Zenject Context ve Manager sınıfları.
* **GAMEPLAY:** Aktif oyun elemanları (Grid, Shapes, InputHandler).
* **UI:** Kullanıcı arayüzü ve EventSystem.