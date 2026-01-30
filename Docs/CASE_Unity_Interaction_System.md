# Ludu Arts - Unity Developer Intern Case

## World Interaction System

**Pozisyon:** Unity Developer Intern
**Süre:** 12 Saat
**Zorluk:** Orta
**Teslim:** GitHub Repository

---

## Genel Bakış

Bu case'de, oyuncunun dünya içindeki nesnelerle etkileşime geçebileceği **modüler bir Interaction System** oluşturmanız beklenmektedir.

Case boyunca:
- Ludu Arts kodlama standartlarına uymanız
- LLM araçlarını etkin kullanmanız
- Temiz, sürdürülebilir ve genişletilebilir kod yazmanız

değerlendirilecektir.

---

## Ludu Arts Standartları

Bu case'de aşağıdaki standart dokümanlarına **tam uyum** beklenmektedir:

| Doküman | Açıklama |
|---------|----------|
| `CSharp_Coding_Conventions.md` | C# kodlama kuralları, prefix'ler, region kullanımı |
| `Naming_Convention_Kilavuzu.md` | Asset ve dosya isimlendirme kuralları |
| `Prefab_Asset_Kurallari.md` | Prefab yapısı, collider kuralları, hierarchy |

**Kritik Kurallar Özeti:**
- Private field'lar için `m_` prefix'i kullanın
- Private static field'lar için `s_` prefix'i kullanın
- Private const'lar için `k_` prefix'i kullanın
- Prefab'ler `P_` ile başlamalı
- Material'lar `M_` ile başlamalı
- ScriptableObject asset'leri uygun prefix ile oluşturulmalı
- Region'lar standart sırayla kullanılmalı
- Public API'ler XML documentation içermeli
- Silent bypass yapılmamalı (hatalar loglanmalı)

---

## Zorunlu Gereksinimler (Must Have)

### 1. Core Interaction System

Temel etkileşim altyapısını oluşturun:

```
Beklenen Yapı:
- IInteractable interface
- InteractionDetector (raycast veya trigger-based)
- Interaction range kontrolü
- Single interaction point (aynı anda tek nesne ile etkileşim)
```

**Teknik Detaylar:**
- Oyuncu belirli bir mesafeden nesnelerle etkileşime geçebilmeli
- Birden fazla interactable aynı range'de ise en yakın olanı seçilmeli
- Etkileşim input'u configurable olmalı (Inspector'dan değiştirilebilir)

### 2. Interaction Types (En az 3 tür)

| Tür | Açıklama | Örnek Kullanım |
|-----|----------|----------------|
| **Instant** | Tek tuş basımı ile anında | Pickup item, button press |
| **Hold** | Basılı tutma gerektiren | Chest açma, kapı kilidi kırma |
| **Toggle** | Açık/kapalı durumlar | Light switch, door |

Her interaction type için base class veya interface oluşturulmalıdır.

### 3. Interactable Objects (En az 4 nesne)

Aşağıdaki interactable nesneleri implement edin:

#### 3.1 Door (Kapı)
- Açılıp kapanabilen kapı
- Locked/Unlocked state
- Kilitli ise "Anahtar gerekli" mesajı
- Toggle interaction type

#### 3.2 Key Pickup (Anahtar)
- Instant interaction ile toplanabilir
- Basit envantere eklenmeli
- Farklı kapılar için farklı anahtarlar (en az 2 tip)

#### 3.3 Switch/Lever (Anahtar/Kol)
- Toggle interaction
- Başka bir nesneyi tetikleyebilmeli (örn: kapıyı açar)
- Event-based connection

#### 3.4 Chest/Container (Sandık)
- Hold interaction ile açılır (örn: 2 saniye basılı tut)
- İçinde item bulunabilir
- Açıldıktan sonra tekrar açılamaz

### 4. UI Feedback

Kullanıcıya görsel geri bildirim sağlayın:

| Özellik | Açıklama |
|---------|----------|
| Interaction Prompt | "Press E to Open" gibi dinamik text |
| Dynamic Text | Nesneye göre değişen mesaj |
| Hold Progress Bar | Basılı tutma için ilerleme göstergesi |
| Out of Range | Menzil dışı feedback |
| Cannot Interact | Etkileşim yapılamıyor feedback (örn: kilitli kapı) |

### 5. Simple Inventory

Basit bir envanter sistemi:

- Key toplama ve saklama
- Locked door + key kontrolü
- Toplanan item'ların UI'da listelenmesi (basit liste yeterli)
- ScriptableObject ile item tanımları

---

## Bonus Gereksinimler (Nice to Have)

Aşağıdaki özellikler ek puan getirir:

| Özellik | Puan |
|---------|------|
| Animation entegrasyonu (kapı açılma, chest açılma) | +3 |
| Sound effects integration points | +2 |
| Multiple keys / color-coded locks | +2 |
| Interaction highlight (outline veya material swap) | +3 |
| Save/Load interaction states | +3 |
| Chained interactions (switch -> door açılır) | +2 |

---

## Repository Yapısı

Aşağıdaki klasör yapısına uyun:

```
📁 InteractionSystem/
├── 📁 Assets/
│   ├── 📁 [ProjectName]/
│   │   ├── 📁 Scripts/
│   │   │   ├── 📁 Runtime/
│   │   │   │   ├── 📁 Core/           # IInteractable, base classes
│   │   │   │   ├── 📁 Interactables/  # Door, Chest, Switch, Key
│   │   │   │   ├── 📁 Player/         # InteractionDetector, Inventory
│   │   │   │   └── 📁 UI/             # InteractionPrompt, ProgressBar
│   │   │   └── 📁 Editor/             # (varsa) custom editor'lar
│   │   ├── 📁 ScriptableObjects/
│   │   │   └── 📁 Items/              # Key definitions
│   │   ├── 📁 Prefabs/
│   │   │   ├── 📁 Interactables/      # P_Door, P_Chest, P_Switch
│   │   │   ├── 📁 UI/                 # P_UI_InteractionPrompt
│   │   │   └── 📁 Player/             # P_Player (varsa)
│   │   ├── 📁 Materials/
│   │   └── 📁 Scenes/
│   │       └── TestScene.unity        # Demo sahne
│   └── 📁 _Dev/                       # WIP dosyaları (varsa)
├── 📁 Docs/                           # Verilen standart dokümanları
│   ├── CSharp_Coding_Conventions.md
│   ├── Naming_Convention_Kilavuzu.md
│   └── Prefab_Asset_Kurallari.md
├── 📄 README.md                       # Proje açıklaması
├── 📄 PROMPTS.md                      # LLM kullanım dokümantasyonu ⭐
└── 📄 .gitignore
```

---

## PROMPTS.md Formatı (Zorunlu)

LLM kullanımınızı aşağıdaki formatta belgeleyin:

```markdown
# LLM Kullanım Dokümantasyonu

## Özet
- Toplam prompt sayısı: X
- Kullanılan araçlar: ChatGPT / Claude / Copilot
- En çok yardım alınan konular: [liste]

---

## Prompt 1: [Konu Başlığı]

**Araç:** ChatGPT-4 / Claude / Copilot
**Tarih/Saat:** YYYY-MM-DD HH:MM

**Prompt:**
> [Yazdığınız prompt - tam metin]

**Alınan Cevap (Özet):**
> [Cevabın özeti veya önemli kısımları]

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> [Neden bu şekilde kullandığınızı açıklayın]

---

## Prompt 2: ...
```

**Önemli:**
- Her önemli LLM etkileşimini kaydedin
- Copy-paste değil, anlayarak kullandığınızı gösterin
- LLM'in hatalı cevap verdiği durumları da belirtin

---

## README.md İçeriği

README dosyanız aşağıdaki bölümleri içermelidir:

```markdown
# Interaction System - [Adınız]

## Kurulum
- Unity versiyonu: [X.X.X]
- Nasıl açılır / çalıştırılır

## Nasıl Test Edilir
- TestScene'i açın
- Kontroller: [WASD, E, vb.]
- Test senaryoları

## Mimari Kararlar
- Neden bu yapıyı seçtim
- Alternatifler neydi
- Trade-off'lar

## Ludu Arts Standartlarına Uyum
- Hangi standartları uyguladım
- Zorlandığım noktalar

## Bilinen Limitasyonlar
- Tamamlayamadığım özellikler
- Bilinen bug'lar
- İyileştirme önerileri

## Ekstra Özellikler
- Bonus olarak eklediklerim
```

---

## Değerlendirme Kriterleri

### Puan Dağılımı (100 Puan)

| Kriter | Puan | Açıklama |
|--------|------|----------|
| **Core System** | 20 | Detection, prompt, basic interaction çalışıyor |
| **4 Interactable** | 20 | Door, Key, Switch, Chest tam implement |
| **UI Feedback** | 10 | Prompt, hold progress, dynamic text |
| **Ludu Arts Standartları** | 20 | Coding conventions, naming, prefab rules |
| **LLM Documentation** | 15 | PROMPTS.md kalitesi ve dürüstlüğü |
| **Bonus Features** | 15 | Ekstra özellikler, polish |
| **TOPLAM** | 100 | |

### Ludu Arts Standartları Detay (20 Puan)

| Alt Kriter | Puan |
|------------|------|
| Field prefix'leri doğru (m_, s_, k_) | 4 |
| Region kullanımı ve sıralaması | 3 |
| XML documentation (public API) | 3 |
| Naming convention (P_, M_, T_) | 4 |
| Prefab yapısı (hierarchy, collider) | 3 |
| Silent bypass yok, error handling | 3 |

### Otomatik Eleme Kriterleri

Aşağıdaki durumlar doğrudan elemeye yol açar:

| Kriter | Açıklama |
|--------|----------|
| ❌ PROMPTS.md yok veya boş | LLM kullanımı belgelenmemiş |
| ❌ Proje açılmıyor | Compile error, missing references |
| ❌ Hiçbir interaction çalışmıyor | Core sistem implement edilmemiş |
| ❌ Tek commit | Geliştirme süreci takip edilemiyor |
| ❌ Plagiarism | Başka projeden birebir kopya |
| ❌ Süre aşımı | 12 saatten fazla süre kullanımı |

---

## Zaman Yönetimi Önerisi

| Saat | Aktivite |
|------|----------|
| 0-1.5 | Dokümanları oku, mimari planla, proje setup |
| 1.5-4 | Core system (IInteractable, Detector) |
| 4-7 | Interactable objects (Door, Key, Switch, Chest) |
| 7-9 | UI feedback sistemi + Simple inventory |
| 9-11 | Test, debug, polish |
| 11-12 | Documentation, commit cleanup, son kontroller |

---

## LLM Kullanım İpuçları

### Etkili Kullanım
```
✅ "Unity'de IInteractable interface'i nasıl tasarlamalıyım?
   Detection için raycast mı trigger mı kullanmalıyım?
   Trade-off'ları açıklar mısın?"

✅ "Bu kodu Ludu Arts C# convention'larına göre refactor et:
   - m_ prefix kullan
   - Region'ları ekle
   - XML documentation ekle"

✅ "Hold interaction için progress tracking nasıl implement edilir?
   Update vs Coroutine hangisi daha uygun?"
```

### Kaçınılması Gereken Kullanım
```
❌ "Bana Unity interaction system yaz" (çok genel)

❌ Kodu anlamadan kopyala-yapıştır

❌ Hata alınca sadece hatayı yapıştırıp "düzelt" demek
```

---

## Teslim Kontrol Listesi

Teslim etmeden önce kontrol edin:

### Repository
- [ ] Repository public
- [ ] .gitignore ekli (Library, Temp, vb. hariç)
- [ ] Commit history mantıklı ve temiz
- [ ] En az 5+ commit var

### Dokümanlar
- [ ] README.md eksiksiz
- [ ] PROMPTS.md detaylı ve dürüst
- [ ] Ludu Arts dokümanları Docs/ klasöründe

### Proje
- [ ] Unity projesi hatasız açılıyor
- [ ] TestScene çalışıyor
- [ ] 4 interactable implement edildi
- [ ] UI feedback çalışıyor

### Standartlar
- [ ] C# coding conventions uygulandı
- [ ] Naming convention'lar doğru
- [ ] Prefab yapısı kurallara uygun

---

## Teslim

Case'i tamamladığınızda:

1. GitHub repository'nizi **public** yapın
2. Repository linkini **aygun@luduarts.com** adresine gönderin
3. Mail konusu: **"Unity Intern Case - [Adınız Soyadınız]"**

---

## Sorular?

Case ile ilgili teknik sorularınız için:
**aygun@luduarts.com**

**Not:** Case içeriği ve gereksinimler hakkında sorular yanıtlanmayacaktır.
Sadece teknik sorunlar (Unity versiyonu, teslim formatı vb.) için iletişime geçin.

---

**Başarılar dileriz!**
*Ludu Arts Team*
