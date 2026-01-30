# Prefab ve Asset Kuralları

Bu kılavuz, Unity projelerinde prefab ve asset'lerin nasıl yapılandırılacağını ve organize edileceğini açıklar.

---

## İçindekiler

1. [Genel Kurallar](#genel-kurallar)
2. [Pivot ve Yönlendirme](#pivot-ve-yönlendirme)
3. [Prefab Hierarchy Yapısı](#prefab-hierarchy-yapısı)
   - [Static Mesh Prefab](#static-mesh-prefab)
   - [Karakter Prefab](#karakter-prefab)
4. [LOD Kuralları](#lod-kuralları)
5. [Collider Kuralları](#collider-kuralları)
6. [Prefab Varyantları](#prefab-varyantları)
7. [Nested Prefab Kullanımı](#nested-prefab-kullanımı)
8. [İsimlendirme Kuralları](#i̇simlendirme-kuralları)
9. [Klasör Yapısı](#klasör-yapısı)
10. [Kontrol Listesi](#kontrol-listesi)

---

## Genel Kurallar

### Temel İlkeler

- Tüm asset'ler ve prefab'ler standart, okunabilir ve sürdürülebilir bir yapıya göre organize edilmelidir
- Prefab'ler sahneye bırakıldığında **doğrudan kullanılabilir** olmalı; ek işlem gerektirmemelidir
- Her prefab Unity sahnesinde konumlandırıldığında aşağıdaki değerlere sahip olmalıdır:

| Özellik | Değer |
|---------|-------|
| Position | (0, 0, 0) |
| Rotation | (0, 0, 0) |
| Scale | (1, 1, 1) |

<!-- TODO: Resim eklenecek - 01_Prefab_Transform_Values.png -->
<!-- Unity Inspector'da Position, Rotation, Scale değerleri (0,0,0), (0,0,0), (1,1,1) -->

---

## Pivot ve Yönlendirme

### Pivot Noktası

| Obje Tipi | Pivot Konumu |
|-----------|--------------|
| Normal modeller | Bottom Center (alt merkez) |
| Dönen objeler | Center (merkez) |
| Kapılar | Menteşe noktası |

### Yönlendirme

- İleri yön (forward direction) her zaman **+Z ekseni** olmalıdır (Unity standardı)
- Prefab sahneye yerleştirildiğinde:
  - Tabanı zemine oturmalı
  - Yönü sahne düzenine uygun olmalı

<!-- TODO: Resim eklenecek - 02_Pivot_Forward_Direction.png -->
<!-- Unity Scene view'da bir objenin pivot noktası ve +Z yönü gösterimi -->

---

## Prefab Hierarchy Yapısı

### Static Mesh Prefab

Statik (hareket etmeyen) objeler için standart yapı:

```
P_ObjectName (Root)
├── Mesh (veya Visual)
├── Colliders
│   ├── Collider_Main
│   └── Collider_Detail (gerekirse)
└── VFX (isteğe bağlı)
```

<!-- TODO: Resim eklenecek - 03_Static_Prefab_Hierarchy.png -->
<!-- Unity Hierarchy'de bir static mesh prefab'ın yapısı -->

**Açıklama:**

| Obje | Açıklama |
|------|----------|
| Root | Ana prefab objesi, transform değerleri sıfırlanmış |
| Mesh | Görsel mesh objesi |
| Colliders | Collider objeleri grubu |
| VFX | Partikül efektleri (opsiyonel) |

---

### Karakter Prefab

Skeletal mesh (rigli) karakterler için yapı:

```
P_CharacterName (Root)
├── [Components: Animator, Controller, Logic Scripts, Collider]
├── SkinnedMeshRenderer (veya SM_Group)
│   ├── Body
│   ├── Head
│   └── Clothes (varyantlar için)
├── Rig_Root
│   └── [Bone hierarchy]
└── Extras
    ├── IK_Points
    ├── VFX_Attach_Points
    └── Extra_Colliders (ragdoll vb.)
```

<!-- TODO: Resim eklenecek - 04_Character_Prefab_Hierarchy.png -->
<!-- Unity Hierarchy'de bir karakter prefab'ın yapısı -->

**Parent-Child İlişkisi:**

| Seviye | İçerik |
|--------|--------|
| 1 (Root) | Karakter Prefab - Animator, Controller, Logic Scripts, Ana Collider |
| 2 | SkinnedMeshRenderer veya SM parent'ı |
| 2 | Rig root (iskelet kökü) |
| 2 | Diğer logical component'ler (IK Points, ekstra collider'lar) |

**Root Üzerindeki Component'ler:**
- Animator
- CharacterController veya Rigidbody + Collider
- Hareket/kontrol scriptleri
- AI scriptleri (NPC için)

---

## LOD Kuralları

### LOD Seviyeleri

| Obje Tipi | LOD Sayısı | Açıklama |
|-----------|------------|----------|
| Normal mesh | 3 (LOD0, LOD1, LOD2) | Çoğu obje için yeterli |
| Büyük objeler (bina vb.) | 4-5 | Uzaktan görülebilen objeler |
| Küçük prop'lar | 2 | Detay gerektirmeyen objeler |

### LOD İsimlendirme

```
SM_Rock_LOD0    # En yüksek detay (yakın)
SM_Rock_LOD1    # Orta detay
SM_Rock_LOD2    # Düşük detay (uzak)
```

<!-- TODO: Resim eklenecek - 05_LOD_Group_Settings.png -->
<!-- Unity'de LOD Group component ayarları -->

### LOD Mesafe Önerileri

| LOD | Ekran Yüzdesi | Kullanım |
|-----|---------------|----------|
| LOD0 | %100 - %50 | Yakın mesafe |
| LOD1 | %50 - %20 | Orta mesafe |
| LOD2 | %20 - %5 | Uzak mesafe |
| Culled | %5 altı | Görünmez |

---

## Collider Kuralları

### Collider Tercih Sırası

Performans için basit collider'lar tercih edilmelidir:

| Öncelik | Collider Tipi | Kullanım Alanı |
|---------|---------------|----------------|
| 1 | Box Collider | Dikdörtgen objeler |
| 2 | Capsule Collider | Silindirik objeler, karakterler |
| 3 | Sphere Collider | Küresel objeler |
| 4 | Mesh Collider (Low-Poly) | Fizik önemli kompleks objeler (araç vb.) |
| 5 | Mesh Collider (High-Poly) | Son çare, sadece static objeler |

<!-- TODO: Resim eklenecek - 06_Collider_Types_Comparison.png -->
<!-- Farklı collider tiplerinin karşılaştırmalı görseli -->

### Proxy Volume (Low-Poly Mesh Collider)

Fiziksel etkileşimin önemli olduğu kompleks objeler (araç, gemi vb.) için **proxy volume** kullanılabilir:

| Özellik | Açıklama |
|---------|----------|
| Nedir? | Objenin düşük poligonlu versiyonu, sadece fizik için kullanılır |
| Ne zaman? | Collision doğruluğu önemli ama basit collider yetersiz kaldığında |
| Nasıl? | 3D yazılımda low-poly collision mesh oluşturulur |
| Görünürlük | Render edilmez, sadece collider olarak kullanılır |

**Örnek Kullanım:**
```
P_Vehicle (Root)
├── Visual
│   └── SM_Car_High (render edilir)
├── Colliders
│   └── SM_Car_Collision (low-poly, MeshCollider, render kapalı)
└── Wheels
    └── [Wheel colliders]
```

**Proxy Volume Ne Zaman Kullanılmalı?**

| Durum | Öneri |
|-------|-------|
| Araba, gemi, uçak | ✅ Proxy volume kullan |
| Karmaşık şekilli interaktif objeler | ✅ Proxy volume kullan |
| Basit prop'lar | ❌ Box/Capsule/Sphere yeterli |
| Static dekoratif objeler | ❌ Basit collider veya collider yok |

### Static Objeler için Collider

- Kompleks değilse: Collider **root üzerinde** olmalı
- Kompleks ise: `Colliders` child objesi altında gruplanmalı
- Ekstra rotasyon gerekiyorsa: Child'da ayrı collider
- Çok kompleks static objeler: Low-poly mesh collider (Convex kapalı)

### Dynamic Objeler için Collider

| Kural | Açıklama |
|-------|----------|
| Tek collider | Olabildiğince tek collider kullanılmalı |
| Rigidbody zorunlu | Her hareket eden collider'da Rigidbody olmalı |
| Compound collider | Gerekirse birden fazla child collider |
| Mesh Collider | Convex **açık** olmalı (dynamic objeler için zorunlu) |

**Önemli:** Dynamic objelerde Mesh Collider kullanılacaksa **Convex** seçeneği mutlaka aktif olmalıdır. Non-convex mesh collider sadece static objeler için çalışır.

### Karakter Collider'ları

| Hareket Tipi | Collider Yapısı |
|--------------|-----------------|
| Fizik tabanlı | Rigidbody + Capsule Collider |
| Kinematik | Rigidbody (Kinematic) + CharacterController |
| Basit | CharacterController |

**Not:** Ragdoll için her kemik üzerinde ayrı collider ve Rigidbody bulunmalıdır.

---

## Prefab Varyantları

### Prefab Variant Kullanımı

- Ana prefab'ten türetilen varyantlar **Prefab Variant** sistemi ile yapılmalı
- Varyantlarda yalnızca gerekli override işlemleri yapılmalı
- Ana prefab bozulmamalı

### İsimlendirme

```
P_Enemy.prefab           # Ana prefab
PV_Enemy_Fire.prefab     # Ateş varyantı
PV_Enemy_Ice.prefab      # Buz varyantı
PV_Enemy_Boss.prefab     # Boss varyantı
```

<!-- TODO: Resim eklenecek - 07_Prefab_Variant_Override.png -->
<!-- Unity'de Prefab Variant override görünümü -->

### Ne Zaman Variant Kullanılmalı?

| Durum | Öneri |
|-------|-------|
| Aynı mesh, farklı material | Variant kullan |
| Aynı yapı, farklı boyut | Variant kullan |
| Aynı temel, farklı component | Variant kullan |
| Tamamen farklı mesh | Yeni prefab oluştur |

---

## Nested Prefab Kullanımı

Prefab içinde prefab kullanımı desteklenir.

### Kullanım Alanları

| Alan | Örnek |
|------|-------|
| UI | Panel içinde Button, Slider, Toggle prefabları |
| Araçlar | Araç içinde tekerlek prefabları |
| Modüler yapılar | Bina içinde kapı, pencere prefabları |
| Customization | Base body + giysi/aksesuar prefabları |

### Örnek: UI Nested Prefab

```
P_UI_SettingsPanel (Root)
├── P_UI_Header
├── P_UI_SettingRow_Audio
│   ├── P_UI_Slider
│   └── P_UI_Toggle
├── P_UI_SettingRow_Graphics
│   ├── P_UI_Dropdown
│   └── P_UI_Toggle
└── P_UI_Button_Apply
```

### Örnek: Runtime Customization

```
P_Character_Base (Runtime'da oluşturulur)
├── SK_Body_Base (her zaman var)
├── [Runtime] SK_Clothes_Shirt
├── [Runtime] SK_Clothes_Pants
├── [Runtime] SK_Accessory_Hat
└── [Runtime] SK_Accessory_Glasses
```

**Not:** Customization prefabları runtime'da base üzerine giydirilir.

---

## İsimlendirme Kuralları

### Prefab İsimlendirme

| Asset Type | Prefix | Suffix | Örnek |
|------------|--------|--------|-------|
| Prefab | P_ | | P_Enemy_01 |
| Prefab Variant | PV_ | | PV_Enemy_Fire |
| UI Prefab | P_UI_ | | P_UI_Button |
| Collider Object | | _Collision | Collider_Main_Collision |
| LOD Mesh | | _LODx | SM_Rock_LOD0 |

### Child Obje İsimlendirme

```
P_Character (Root)
├── Visual                    # Görsel grup
├── Colliders                 # Collider grup
├── VFX                       # Efekt grup
├── Audio                     # Ses grup
└── Points                    # Referans noktaları
    ├── Point_Head
    ├── Point_Hand_L
    └── Point_Hand_R
```

---

## Klasör Yapısı

### Proje Klasör Yapısı

```
📁 Assets/{ProjectName}/
├── Characters/
│   └── {CharacterName}/
│       ├── Animations/
│       ├── Materials/
│       ├── Textures/
│       ├── SK_{CharacterName}.fbx
│       └── P_{CharacterName}.prefab
├── Objects/
│   ├── Architecture/
│   │   └── {BuildingName}/
│   └── Props/
│       └── {PropName}/
├── Vehicles/
│   └── {VehicleName}/
├── Weapons/
│   └── {WeaponName}/
├── FX/
│   └── Particles/
├── UI/
│   └── Prefabs/
├── _Levels/
│   └── {LevelName}/
├── MaterialLibrary/
│   └── Shaders/
└── Lighting/
    ├── HDRI/
    └── Lut/
```

### Karakter Klasör Örneği

```
📁 Characters/Patient/
├── Animations/
│   ├── A_Patient_Idle.fbx
│   ├── A_Patient_Walk.fbx
│   └── AC_Patient.controller
├── Materials/
│   ├── M_Patient_Body.mat
│   └── M_Patient_Clothes.mat
├── Textures/
│   ├── T_Patient_Body_BC.png
│   ├── T_Patient_Body_N.png
│   └── T_Patient_Body_ORM.png
├── SK_Patient.fbx
└── P_Patient.prefab
```

---

## Kontrol Listesi

### Prefab Oluşturma Kontrol Listesi

**Transform:**
- [ ] Position: (0, 0, 0)
- [ ] Rotation: (0, 0, 0)
- [ ] Scale: (1, 1, 1)

**Pivot ve Yön:**
- [ ] Pivot bottom-center'da mı?
- [ ] İleri yön +Z mi?
- [ ] Taban zemine oturuyor mu?

**Hierarchy:**
- [ ] Root obje doğru isimlendirildi mi? (P_ prefix)
- [ ] Child objeler mantıklı gruplanmış mı?
- [ ] Gereksiz boş obje yok mu?

**Collider:**
- [ ] Uygun collider tipi seçildi mi?
- [ ] Collider boyutu mesh'e uygun mu?
- [ ] Dynamic objede Rigidbody var mı?

**LOD:**
- [ ] LOD Group eklendi mi? (gerekiyorsa)
- [ ] LOD seviyeleri doğru ayarlandı mı?
- [ ] LOD mesh'leri doğru isimlendirildi mi?

**Genel:**
- [ ] Prefab sahneye bırakıldığında çalışıyor mu?
- [ ] Gereksiz component yok mu?
- [ ] Material ve texture bağlantıları doğru mu?

---

## Sık Karşılaşılan Hatalar

### 1. Transform Değerleri Sıfır Değil

**Problem:** Prefab'in position/rotation/scale değerleri sıfırlanmamış.

**Çözüm:**
1. Prefab'i sahneye koy
2. Transform değerlerini sıfırla
3. Apply changes to prefab

### 2. Pivot Yanlış Konumda

**Problem:** Obje sahneye konulduğunda havada kalıyor veya zemine gömülüyor.

**Çözüm:**
1. 3D yazılımda pivot'u düzelt
2. Yeniden export et

### 3. Yön Yanlış

**Problem:** Karakter/araç yanlış yöne bakıyor.

**Çözüm:**
1. 3D yazılımda rotasyonu düzelt (+Z ileri)
2. Yeniden export et

### 4. Collider Çok Kompleks

**Problem:** Mesh Collider performans sorunu yaratıyor.

**Çözüm:**
1. Basit collider'lar kullan (Box, Capsule, Sphere)
2. Gerekirse birden fazla basit collider kombine et

### 5. LOD Eksik

**Problem:** Uzaktaki objeler çok detaylı, performans düşük.

**Çözüm:**
1. LOD Group ekle
2. En az 2-3 LOD seviyesi oluştur

---

## Özet

| Konu | Kural |
|------|-------|
| Transform | (0,0,0), (0,0,0), (1,1,1) |
| Pivot | Bottom-center (normal), Center (dönen) |
| İleri Yön | +Z ekseni |
| LOD | 3 seviye (normal), 4-5 (büyük objeler) |
| Collider | Box > Capsule > Sphere > Mesh |
| Variant | Override sadece gerekli yerlerde |
| Nested | Modüler yapılar için kullanılabilir |

---

*Bu doküman Ludu Arts şirketi içi kullanım için hazırlanmıştır.*
*Sorularınız için Lead Developer ile iletişime geçin.*
