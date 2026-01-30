# Naming Convention Kılavuzu

Bu kılavuz, Unity projelerinde tutarlı ve okunabilir bir isimlendirme standardı oluşturmak için hazırlanmıştır. Tüm geliştiriciler bu kurallara uymalıdır.

---

## İçindekiler

1. [Genel Kurallar](#genel-kurallar)
2. [Asset İsimlendirme Kuralları](#asset-i̇simlendirme-kuralları)
   - [Mesh Dosyaları](#mesh-dosyaları)
   - [Prefab Dosyaları](#prefab-dosyaları)
   - [Material Dosyaları](#material-dosyaları)
   - [Texture Dosyaları](#texture-dosyaları)
   - [Animasyon Dosyaları](#animasyon-dosyaları)
   - [Ses Dosyaları](#ses-dosyaları)
   - [VFX Dosyaları](#vfx-dosyaları)
3. [Numaralandırma Kuralları](#numaralandırma-kuralları)
4. [Klasör Yapısı](#klasör-yapısı)
5. [Sahne Hiyerarşisi](#sahne-hiyerarşisi)
6. [Örnekler](#örnekler)

---

## Genel Kurallar

### Temel İlkeler

- Tüm adlandırmalar **İngilizce** yapılmalıdır
- Anlamlı, açık ve kısa isimler tercih edilmelidir
- Kısaltmalardan kaçınılmalı; gerekiyorsa proje genelinde standartlaştırılmalıdır
- Özel karakterler (@ # $ % & *) kullanılmamalıdır
- Sadece `_` (underscore) gerektiğinde kullanılabilir

### Kelime Ayırma Kuralları

| Durum | Format | Örnek |
|-------|--------|-------|
| Kısa isimler | PascalCase | `PlayerController` |
| Uzun isimler | Pascal_Case | `Player_Movement_Controller` |

**Kural:** İsim okunması zorlaşacak kadar uzunsa underscore ile ayırın.

### Klasör İsimlendirme

- Her klasör ismi **PascalCase** olmalıdır
- Örnek: `GameSystems`, `UI`, `Weapons`, `Environment`

---

## Asset İsimlendirme Kuralları

### Mesh Dosyaları

![Skeletal Mesh Örnekleri](src/6.png)

| Asset Türü | Prefix | Örnek | Açıklama |
|------------|--------|-------|----------|
| Skeletal Mesh | SK_ | SK_Body_01 | Rig içeren mesh |
| Static Mesh | SM_ | SM_Rock_01 | Statik obje |

![Static Mesh Örnekleri](src/1.png)

**Örnekler:**
```
SK_BaseBody
SK_Body_01
SK_Body_02

SM_Plunger
SM_RabbitToy_01
SM_Saw_01
SM_Square
```

---

### Prefab Dosyaları

![Prefab Örnekleri](src/8.png)

| Asset Türü | Prefix | Örnek | Açıklama |
|------------|--------|-------|----------|
| Prefab | P_ | P_Ambulance_01 | Ana prefab |
| Prefab Variant | PV_ | PV_Ambulance_Night | Varyant prefab |
| UI Prefab | P_UI_ | P_UI_SettingBarButton | UI prefabları |

![UI Prefab Örnekleri](src/9.png)

**Örnekler:**
```
P_Ambulance_001
P_AmbulanceInside_001
P_BearToy_001
P_Blood
P_BurnReliefSpray_001
P_Cloud

P_UI_SettingBarButton
P_UI_SettingBarDropdown
P_UI_SettingBarSlider
P_UI_Toggle_Element
```

---

### Material Dosyaları

| Asset Türü | Prefix | Örnek | Açıklama |
|------------|--------|-------|----------|
| Material | M_ | M_Rock | Ana materyal |
| Material Variant | MV_ | MV_Rock_Snow | Materyal varyantı |

**Örnekler:**
```
M_Character_Skin
M_Environment_Ground
MV_Ground_Wet
MV_Ground_Snow
```

---

### Texture Dosyaları

![Texture Örnekleri](src/5.png)

| Texture Türü | Prefix + Suffix | Örnek | Açıklama |
|--------------|-----------------|-------|----------|
| Base Color / Diffuse | T_ + _BC veya _D | T_Anakin_BC | Temel renk |
| Normal Map | T_ + _N | T_Anakin_N | Normal haritası |
| Metallic/Smoothness | T_ + _MS | T_Anakin_MS | Metalik/pürüzsüzlük |
| Ambient Occlusion | T_ + _AO | T_Anakin_AO | AO haritası |
| Emissive | T_ + _E | T_Anakin_E | Emissive haritası |
| Alpha | T_ + _A | T_Anakin_A | Alfa kanalı |
| Height | T_ + _H | T_Anakin_H | Yükseklik haritası |
| Mask | T_ + _M | T_Anakin_M | Maske haritası |
| Mask Map (Packed) | T_ + _Mask | T_Anakin_Mask | Metallic+AO+Detail+Smoothness |
| UI Texture | T_ + _GUI | T_Icon_GUI | UI sprite'ları |
| Cubemap | TC_ | TC_Skybox_01 | Cubemap |
| Media Texture | MT_ | MT_VideoMonitor | Video texture |
| Render Target | RT_ | RT_Reflection | Render target |

![UI Texture Örnekleri](src/7.png)

**Örnekler:**
```
T_BilboardC1_BC
T_Cornea_BC
T_LowerTeeth_BC
T_Tongue_BC

T_Header_MedicalShop_GUI
T_HeaderBar_Player_Active_GUI
T_Healthbar_GUI
T_Hud_Healthbar_GUI
T_Icon_Adrenalin_GUI
```

---

### Animasyon Dosyaları

![Animation Örnekleri](src/0.png)

| Asset Türü | Prefix | Örnek | Açıklama |
|------------|--------|-------|----------|
| Animation Clip | A_ | A_Adrenaline | Animasyon clip |
| Animation Controller | AC_ | AC_Adrenaline | Animator Controller |
| Avatar Mask | AM_ | AM_UpperBody | Avatar maskesi |
| Morph Target | MT_ | MT_FaceSmile | Blendshape |

**Örnekler:**
```
A_Adrenaline
A_Collectable_Blood

AC_Adrenaline
AC_Collectable_Blood
AC_FPS_Hand_Tap_Button_R
AC_OxygenTube
AC_Stretcher
```

---

### Ses Dosyaları

![SFX Örnekleri](src/2.png)

| Asset Türü | Prefix | Örnek | Açıklama |
|------------|--------|-------|----------|
| Sound Effect | SFX_ | SFX_Explosion_01 | Ses efekti |
| Background Music | BGM_ | BGM_Level_01 | Arka plan müziği |
| Voice Over | VO_ | VO_Character_Line_01 | Seslendirme |

![BGM Örnekleri](src/3.png)

**Örnekler:**
```
SFX_Body_Fall_00
SFX_Body_Fall_01
SFX_Body_Fall_02
SFX_BodyHit_Metal_01

BGM_CityAmbiance_01
BGM_CityAmbiance_03
BGM_MainMenu
```

---

### VFX Dosyaları

![VFX Örnekleri](src/4.png)

| Asset Türü | Prefix | Örnek | Açıklama |
|------------|--------|-------|----------|
| VFX Prefab | VFX_ | VFX_Explosion | Particle/efekt prefabı |

**Örnekler:**
```
VFX_LightRayCube
VFX_Fire_01
VFX_Smoke_01
VFX_Blood_Splatter
```

---

## Numaralandırma Kuralları

### Standart Format

| Durum | Format | Örnek |
|-------|--------|-------|
| 1-99 varyant | _01, _02 | SM_Rock_01, SM_Rock_02 |
| 100+ varyant | _001, _002 | SM_Tile_001, SM_Tile_002 |

### Varyant Örnekleri

```
✅ Doğru:
SM_Saw_01
SM_Saw_02
SM_Saw_03

❌ Yanlış:
SM_SawV1_001
SM_SawV2_001
```

### Numaralandırma Ne Zaman Kullanılır?

| Durum | Kullanım |
|-------|----------|
| Aynı objenin farklı versiyonları | SM_Rock_01, SM_Rock_02 |
| Sıralı animasyonlar | A_Attack_01, A_Attack_02 |
| Ses varyantları | SFX_Footstep_01, SFX_Footstep_02 |
| Tek obje (varyant yok) | SM_Table (numara yok) |

---

## Klasör Yapısı

### Proje Klasör Yapısı

```
📁 Assets/
├── _Dev/
│   └── [DeveloperName]/          # Geliştirici WIP dosyaları
├── [ProjectName]/
│   ├── Characters/               # Karakter model/animasyon
│   ├── FX/
│   │   └── Particles/            # Efekt sistemleri
│   ├── Vehicles/                 # Araçlar
│   ├── Weapons/                  # Silahlar
│   ├── Gameplay/                 # Oynanış sistemleri
│   ├── _Levels/                  # Sahne klasörleri
│   ├── Lighting/                 # Aydınlatma
│   │   ├── HDRI/
│   │   ├── Lut/
│   │   └── Textures/
│   ├── MaterialLibrary/          # Ortak materyaller
│   │   ├── Debug/
│   │   └── Shaders/
│   ├── Objects/
│   │   ├── Architecture/         # Mimari objeler
│   │   └── Props/                # Prop objeler
│   ├── Scripts/                  # Kod dosyaları
│   ├── Sound/                    # Ses dosyaları
│   └── UI/                       # UI kaynakları
│       └── Art/
├── ExpansionPack/                # DLC içerikleri
├── Plugins/                      # Pluginler
└── ThirdPartySDK/                # 3. parti SDK'lar
```

### Karakter Klasör Örneği

```
📁 Characters/
└── Patient/
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
    │   └── T_Patient_Body_Mask.png
    ├── SK_Patient.fbx
    └── P_Patient.prefab
```

---

## Sahne Hiyerarşisi

Unity sahnesinde objelerin organizasyonu:

### Sistem Klasörleri (@ ile başlar - en üstte)

| Klasör | Açıklama |
|--------|----------|
| @System | GameManager, Loader vb. sistem objeleri |
| @Debug | Debug/geliştirme objeleri |
| @Management | Sahne yönetimi objeleri |
| @UI | UI Canvas ve elemanları |

### Sahne Klasörleri

| Klasör | Açıklama |
|--------|----------|
| Layouts | UI yerleşim sistemleri |
| Cameras | Kameralar |
| Lights | Işıklar |
| Volumes | Post Process, Light Probe vb. |
| Particles | VFX efektleri |
| Sound | Audio kaynakları |

### World Klasörü

| Klasör | Açıklama |
|--------|----------|
| World | Sahne dünyası |
| └── Global | Tüm sahneye ait objeler |
| └── Room1 | Bölgesel objeler |
| └── Architecture | Mimari yapılar |
| └── Terrain | Zemin objeleri |
| └── Props | Küçük sahne nesneleri |

### Gameplay Klasörü

| Klasör | Açıklama |
|--------|----------|
| Gameplay | Etkileşimli elemanlar |
| └── Actors | Oyuncu ve NPC'ler |
| └── Items | Eşyalar |
| └── Triggers | Trigger alanları |
| └── Quests | Görev objeleri |

### Özel Klasörler

| Klasör | Açıklama |
|--------|----------|
| _Dynamic | Runtime'da oluşturulan objeler |

---

## Örnekler

### Tam Asset İsimlendirme Örnekleri

**Karakter:**
```
SK_Patient.fbx                    # Skeletal Mesh
P_Patient.prefab                  # Prefab
M_Patient_Body.mat                # Material
T_Patient_Body_BC.png             # Base Color
T_Patient_Body_N.png              # Normal Map
T_Patient_Body_Mask.png           # Mask Map (Packed)
A_Patient_Idle.fbx                # Animation
AC_Patient.controller             # Animator Controller
```

**Ortam Objesi:**
```
SM_Rock_01.fbx                    # Static Mesh
SM_Rock_02.fbx                    # Varyant
P_Rock_01.prefab                  # Prefab
M_Rock.mat                        # Material
T_Rock_BC.png                     # Base Color
T_Rock_N.png                      # Normal Map
```

**UI:**
```
P_UI_HealthBar.prefab             # UI Prefab
T_Icon_Health_GUI.png             # UI Texture
T_Button_Primary_GUI.png          # Button Texture
```

**Ses:**
```
SFX_Footstep_Concrete_01.wav      # Sound Effect
SFX_Footstep_Concrete_02.wav      # Varyant
BGM_Level_01.mp3                  # Background Music
VO_Patient_Scream_01.wav          # Voice Over
```

**VFX:**
```
VFX_Blood_Splatter.prefab         # VFX Prefab
VFX_Fire_01.prefab                # VFX Varyant
```

---

## Özet Tablosu

| Kategori | Prefix | Suffix | Örnek |
|----------|--------|--------|-------|
| Skeletal Mesh | SK_ | | SK_Character_01 |
| Static Mesh | SM_ | | SM_Rock_01 |
| Prefab | P_ | | P_Enemy_01 |
| Prefab Variant | PV_ | | PV_Enemy_Night |
| UI Prefab | P_UI_ | | P_UI_Button |
| Material | M_ | | M_Rock |
| Material Variant | MV_ | | MV_Rock_Wet |
| Texture Base Color | T_ | _BC | T_Rock_BC |
| Texture Normal | T_ | _N | T_Rock_N |
| Texture Mask Map | T_ | _Mask | T_Rock_Mask |
| Texture UI | T_ | _GUI | T_Icon_GUI |
| Animation Clip | A_ | | A_Idle |
| Animation Controller | AC_ | | AC_Player |
| Avatar Mask | AM_ | | AM_UpperBody |
| Sound Effect | SFX_ | | SFX_Explosion_01 |
| Background Music | BGM_ | | BGM_Menu |
| Voice Over | VO_ | | VO_Line_01 |
| VFX | VFX_ | | VFX_Fire_01 |
| Cubemap | TC_ | | TC_Skybox |
| Render Target | RT_ | | RT_Mirror |

---

## Kontrol Listesi

Her asset oluştururken kontrol edin:

- [ ] İsim İngilizce mi?
- [ ] Doğru prefix kullanıldı mı?
- [ ] Doğru suffix kullanıldı mı? (texture'lar için)
- [ ] Numaralandırma formatı doğru mu? (_01 veya _001)
- [ ] Kelime ayırma kuralına uygun mu? (PascalCase veya Pascal_Case)
- [ ] Özel karakter kullanılmadı mı?
- [ ] Doğru klasöre yerleştirildi mi?

---

**Önemli Kurallar:**

- Tüm isimler **İngilizce** olmalı
- Prefix'ler **zorunlu**
- Varyantlar için **_01, _02** formatı kullanılmalı (V1, V2 değil)
- Kısa isimler **PascalCase**, uzun isimler **Pascal_Case**
- UI prefabları **P_UI_** ile başlamalı
- VFX prefabları **VFX_** ile başlamalı

---

*Bu doküman Ludu Arts şirketi içi kullanım için hazırlanmıştır.*
*Sorularınız için Lead Developer ile iletişime geçin.*
