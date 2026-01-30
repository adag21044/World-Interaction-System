# LLM Kullanım Dokümantasyonu

## Özet
- Toplam prompt sayısı: 6
- Kullanılan araçlar: ChatGPT (Codex)
- En çok yardım alınan konular: Input System, interactable davranışları, UI progress/slider, naming convention kontrolü

---

## Prompt 1: Mouse hareketi çalışmıyor

**Araç:** ChatGPT  
**Tarih/Saat:** 2026-01-30 10:15

**Prompt:**
> FPS kamera mouse look çalışmıyor. Input System ve camera root tarafında olası nedenleri ve çözümü öner.

**Alınan Cevap (Özet):**
> CameraRoot fallback (child camera/main camera) ve look input için Pointer cihazlarını mouse gibi kabul etme önerildi ve uygulandı.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Kamera referansı ve input cihaz tipini düzelterek mouse look sorununu giderdim.

---

## Prompt 2: Chest/Container (Hold + One-time) sistemi

**Araç:** ChatGPT  
**Tarih/Saat:** 2026-01-30 10:40

**Prompt:**
> Chest/Container için Hold + One-time açılma sistemi istiyorum. Kapak +X yönünde 0.5f local offset ile açılmalı ve içindeki item interactable açıldığında aktif olmalı. Yapıyı nasıl kurmalıyım?

**Alınan Cevap (Özet):**
> ChestInteractable’da kapak +X local offset ile açılacak şekilde değiştirildi; açılınca içteki interactable aktif edildi; one-time açılış korundu.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Var olan chest davranışını offset-based açılış ve iç interactable aktivasyonu ile genişlettim.

---

## Prompt 3: Trigger alanına girince light aktif + DOTween ile dönme

**Araç:** ChatGPT  
**Tarih/Saat:** 2026-01-30 11:05

**Prompt:**
> Trigger alanına girince bir light objesi aktif olsun ve bir obje DOTween ile sonsuz kendi ekseninde dönsün. Toggle + event-based bir interactable ve receiver yapısı öner.

**Alınan Cevap (Özet):**
> TriggerSwitchInteractable (toggle + event-based) ve LightSpinReceiver (light aktif + DOTween ile sonsuz dönüş) eklendi.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Event-based bağlantı ile switch tetikleyince light ve rotasyon başladı.

---

## Prompt 4: Hold interaction için slider progress

**Araç:** ChatGPT  
**Tarih/Saat:** 2026-01-30 11:45

**Prompt:**
> Hold interaction sırasında ekranda slider aktif olmalı; progress ile senkron ilerlemeli ve tamamlanınca/cancel olunca gizlenmeli. UI tarafını nasıl tasarlamalıyım?

**Alınan Cevap (Özet):**
> InteractionPromptUI’ya Slider referansı eklendi; hold sırasında slider aktif ve değer artıyor; tamamlanınca/cancel olunca gizleniyor.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> UI’da image yerine slider ile net ilerleme göstergesi kullandım.

---

## Prompt 5: Naming convention taraması

**Araç:** ChatGPT  
**Tarih/Saat:** 2026-01-30 12:05  

**Prompt:**
> Assets/WorldInteractionSystem icin prefab/material/texture/mesh naming convention taramasi yap ve sonucu ozetle.

**Alınan Cevap (Özet):**
> Assets/WorldInteractionSystem icin naming convention taramasi yapildi ve sorun bulunmadi.

**Nasıl Kullandım:**
- [ ] Direkt kullandım  
- [x] Adapte ettim  
- [ ] Reddettim  

**Açıklama:**
> Projede uyguladigim isimlendirme kurallarini dogruladim.

---

## Prompt 6: InventoryManager input duzeltme

**Araç:** ChatGPT  
**Tarih/Saat:** 2026-01-30 12:25

**Prompt:**
> InventoryManager icindeki legacy input kullanimini kaldirip Input System'e uygun hale getir.

**Alınan Cevap (Özet):**
> InputActionReference destekli toggle eklendi ve eski Input.GetKeyDown kaldirildi.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Inventory toggle davranisini Input System standartlarina uygun hale getirdim.

---

## Genel değerlendirme
- [x] En yardımcı olduğu alanlar:
  - [x] Input System entegrasyonu ve input aksiyonlarının düzenlenmesi
  - [x] Interactable akislarinin tasarlanmasi (hold/toggle/instant)
  - [x] UI tarafinda progress gosterimi ve durum mesajlari
- [x] Yetersiz kaldığı alanlar (+ neden):
  - [x] Sahne/prefab baglama adimlarinda proje ozelinde varsayim gerekmesi
  - [x] Asset isimlerinin “Ingilizce” olup olmadigini semantik olarak dogrulayamamasi
- [x] LLM kullanımı hakkında düşünceler:
  - [x] Hizli prototip ve kontrol listesi dogrulamasinda verimli
  - [x] Unity sahne duzenlemelerinde insan dogrulamasi gerekiyor
