# 🎵 Bepop Music - AI Destekli Müzik Akış Platformu

![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-purple)
![ML.NET](https://img.shields.io/badge/ML.NET-Machine%20Learning-blue)
![OpenAI](https://img.shields.io/badge/OpenAI-GPT--4o-green)
![Cloudinary](https://img.shields.io/badge/Cloud-Storage-orange)
![License](https://img.shields.io/badge/License-MIT-lightgrey)

**Bepop Music**, kullanıcıların müzik dinleyebileceği, yapay zeka destekli öneriler alabileceği ve abonelik paketlerine göre içeriklere erişebileceği N-Katmanlı mimari ile geliştirilmiş modern bir web uygulamasıdır. 

Proje; **Clean Architecture** prensipleri, **SOLID** kuralları ve **RESTful API** standartları gözetilerek geliştirilmiştir.

---

## 🚀 Proje Özellikleri ve İş Akışı

### 👤 Kullanıcı İşlemleri, Güvenlik ve API Yapısı
* **API Bazlı Yetkilendirme (Token Interception):** Backend, dış dünyaya kapalı bir API yapısına sahiptir. İstemci (Client) tarafından API'ye atılan her istekte **Bearer Token** kontrolü yapılır. Geçerli bir token barındırmayan istekler, Controller katmanına ulaşmadan middleware seviyesinde **401 Unauthorized** hatası ile reddedilir.
* **Güvenli Kayıt:** Kullanıcı şifreleri veritabanında ham (raw) halde tutulmaz; **BCrypt** kütüphanesi ile hashlenerek saklanır.
* **Role-Based Access Control (RBAC):** Admin ve Kullanıcı rolleri token içerisine gömülmüştür. Kullanıcı tokenı ile Admin endpointlerine istek atıldığında sistem **403 Forbidden** döner.

### 💳 Abonelik ve Ödeme Sistemi (Iyzipay)
* **Paket Yönetimi:** Kullanıcı ilk kayıtta paketsiz başlar. İçeriklere erişmek için paket seçimi yapar.
* **Sandbox Ödeme:** **Iyzico** sanal pos entegrasyonu ile güvenli ödeme simülasyonu gerçekleştirilir.
* **Akıllı Yükseltme:** Kullanıcılar mevcut paketlerini yükseltebilir veya düşürebilir; ancak hali hazırda sahip oldukları paketi tekrar satın almaları engellenir.

### 🧠 Yapay Zeka ve Öneri Sistemleri
Proje iki farklı AI teknolojisini barındırır:
1.  **ML.NET Analizi:** Kullanıcı davranışlarına dayalı "Bunu da beğenebilirsiniz" öneri sistemi ve eşleşme oranları sunar.
2.  **BepopDJ (OpenAI GPT-4o):** Kullanıcının ruh haline (mood) göre çalışan akıllı asistan.
    * *Örnek:* Kullanıcı "Hüzünlü ama umutlu" dediğinde, GPT-4o veritabanındaki uygun şarkıları analiz ederek özel bir liste önerir.
    * *Temperature Ayarı:* Yaratıcılık katsayısı (1.0) ile her seferinde çeşitlendirilmiş ve özgün öneriler sunulur.

### 🎧 Müzik Deneyimi ve Kısıtlamalar
* **Keşfet (Discovery):** Trend şarkılar ve son eklenenler vitrini.
* **Erişim Kontrolü:** Her müziğin bir "Level" değeri vardır. Kullanıcının paket seviyesi şarkıyı karşılamıyorsa, sistem dinlemeye izin vermez ve paket yükseltme önerisinde bulunur.
* **Çalma Listeleri:** Kullanıcılar özgürce liste oluşturabilir. *Not:* Listeye her seviyeden şarkı eklenebilir, ancak "Oynat" butonuna basıldığında anlık paket/yetki kontrolü yapılır.

### ☁️ Bulut Tabanlı Dosya Yönetimi
* **Cloudinary Entegrasyonu:** Müzik dosyaları ve kapak görselleri sunucu diskinde değil, Cloudinary bulut depolama servisinde optimize edilmiş şekilde saklanır.

  
  <img width="2659" height="1535" alt="Gemini_Generated_Image_u78wo8u78wo8u78w" src="https://github.com/user-attachments/assets/3490d62b-1ca1-4d40-aac2-b71f5970dd96" />


---

## 🏗️ Mimari ve Teknik Detaylar

Bu proje, sürdürülebilirlik ve ölçeklenebilirlik hedeflenerek **N-Katmanlı Mimari (N-Layer Architecture)** üzerine inşa edilmiştir.

### Kullanılan Teknolojiler ve Kütüphaneler
| Teknoloji | Kullanım Amacı |
| :--- | :--- |
| **.NET 8.0** | Backend Framework |
| **ML.NET** | Makine Öğrenimi ve Öneri Motoru |
| **OpenAI API (GPT-4o)** | BepopDJ Asistanı (Generative AI) |
| **Iyzipay** | Ödeme Sistemi Entegrasyonu |
| **Cloudinary** | Medya Yönetimi ve Depolama |
| **BCrypt.Net-Next** | Şifreleme (Hashing) |
| **JWT (Json Web Token)** | Kimlik Doğrulama |

### Uygulanan Tasarım Desenleri ve Prensipler
* **Clean Code & SOLID:** Kodun okunabilirliği ve bağımlılıkların yönetimi için katı kurallar uygulandı.
* **Options Pattern:** `appsettings.json` içerisindeki konfigürasyonların (API Keyler, Ayarlar) tip güvenli (type-safe) bir şekilde yönetilmesi sağlandı.
* **Service Registration Extension:** `Program.cs` dosyasının şişmesini engellemek için servis bağımlılıkları Business katmanında extension metotlar ile yönetildi.
* **Custom Claims:** Token içerisine taşınan veriler `Constants` klasöründe standartlaştırılarak "Magic String" kullanımından kaçınıldı.

---



## 🔒 Yetkilendirme Matrisi

| İşlem | Ziyaretçi (Token Yok) | Giriş Yapmış Kullanıcı | Admin |
| :--- | :---: | :---: | :---: |
| API Erişimi | ❌ (401 Unauthorized) | ✅ | ✅ |
| Vitrin Görüntüleme | ❌ | ✅ | ✅ |
| Müzik Dinleme | ❌ | ✅ (Paket Dahilinde) | ✅ |
| Playlist Oluşturma | ❌ | ✅ | ✅ |
| Admin Paneli | ❌ | ❌ (403 Forbidden) | ✅ |
| Analizleri Görme | ❌ | ❌ | ✅ |

---

<img width="3807" height="1980" alt="Ekran görüntüsü 2026-01-01 195231" src="https://github.com/user-attachments/assets/d472e3c3-bf54-46f5-92f4-e628e7b13eba" />


<img width="3828" height="1980" alt="Ekran görüntüsü 2026-01-01 195452" src="https://github.com/user-attachments/assets/4526939b-8c5b-4fa0-b133-2c910c3782dd" />
<img width="3839" height="1973" alt="Ekran görüntüsü 2026-01-01 195511" src="https://github.com/user-attachments/assets/5f6f1722-483c-4ca8-a198-f706bba287cb" />
<img width="3838" height="1981" alt="Ekran görüntüsü 2026-01-01 195646" src="https://github.com/user-attachments/assets/54e73b31-68b2-409b-a702-024dd26affa6" />
<img width="3826" height="1972" alt="Ekran görüntüsü 2026-01-01 195733" src="https://github.com/user-attachments/assets/4ce54a19-dce1-4e07-8f59-a4a9c948b2fe" />
<img width="3165" height="1966" alt="Ekran görüntüsü 2026-01-01 195739" src="https://github.com/user-attachments/assets/0df1f985-bbf9-4539-ac65-8bf842a01d28" />
<img width="3816" height="1953" alt="Ekran görüntüsü 2026-01-01 195751" src="https://github.com/user-attachments/assets/0be3835d-839c-4262-a1e3-9109d0e4a6f0" />
<img width="2637" height="1267" alt="Ekran görüntüsü 2026-01-01 200350" src="https://github.com/user-attachments/assets/2f1b547c-6c39-4913-9a8a-a20961a53dd6" />
<img width="3777" height="1959" alt="Ekran görüntüsü 2026-01-01 201518" src="https://github.com/user-attachments/assets/22188756-3e8f-4695-855a-cf8e954c91db" />
<img width="2232" height="696" alt="Ekran görüntüsü 2026-01-01 204127" src="https://github.com/user-attachments/assets/d9d65ca8-cf0c-47e9-a99d-a6212f9088d0" />

<img width="3506" height="1197" alt="Ekran görüntüsü 2026-01-01 201541" src="https://github.com/user-attachments/assets/41aaf08f-ca3f-4775-9834-57569a9c8d85" />
<img width="3797" height="1928" alt="Ekran görüntüsü 2026-01-01 201616" src="https://github.com/user-attachments/assets/41080881-3516-4805-be14-f40d59cdc5a5" />
<img width="3796" height="1968" alt="Ekran görüntüsü 2026-01-01 201711" src="https://github.com/user-attachments/assets/8a573a0a-920a-4d31-9a26-70d1e31d4707" />
<img width="3797" height="1954" alt="Ekran görüntüsü 2026-01-01 201747" src="https://github.com/user-attachments/assets/dc3be275-8883-4b1c-bec5-f8c4d5630753" />
<img width="3811" height="1961" alt="Ekran görüntüsü 2026-01-01 201807" src="https://github.com/user-attachments/assets/0952b083-1fb7-4444-a9be-59c2e9c54bc9" />
<img width="3805" height="1944" alt="Ekran görüntüsü 2026-01-01 201908" src="https://github.com/user-attachments/assets/64c1cf56-5dc5-4c79-a5b1-c9f6afe197d5" />
<img width="3825" height="1974" alt="Ekran görüntüsü 2026-01-01 202018" src="https://github.com/user-attachments/assets/0750f956-a459-4797-907e-4de4acb0b823" />
<img width="3763" height="1800" alt="Ekran görüntüsü 2026-01-01 201948" src="https://github.com/user-attachments/assets/f4ca1d7d-83d8-45d7-8b49-6fd1888edff4" />

<img width="3791" height="1294" alt="Ekran görüntüsü 2026-01-01 212143" src="https://github.com/user-attachments/assets/9dbda569-cdd2-4704-bed7-053f7f46877c" />
<img width="3811" height="1390" alt="Ekran görüntüsü 2026-01-01 212154" src="https://github.com/user-attachments/assets/0c6f67d5-589a-4061-8769-9f0a3ef4bd62" />

<img width="3819" height="1933" alt="Ekran görüntüsü 2026-01-01 212600" src="https://github.com/user-attachments/assets/33c9bddf-731a-4e9d-bac4-3b34a27fb4d0" />
<img width="3802" height="1947" alt="Ekran görüntüsü 2026-01-01 213122" src="https://github.com/user-attachments/assets/42468f74-fad4-4ad9-be78-c5ccb03a4638" />
<img width="343" height="386" alt="Ekran görüntüsü 2026-01-01 213252" src="https://github.com/user-attachments/assets/e6963328-e500-4beb-998a-7dd85e66e6fc" />

