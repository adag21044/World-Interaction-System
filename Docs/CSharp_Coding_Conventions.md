# C# Coding Conventions

Bu kılavuz, Unity projelerinde C# kod yazımı için standartları belirler.

---

## İçindekiler

1. [Genel İlkeler](#genel-i̇lkeler)
2. [İsimlendirme Kuralları](#i̇simlendirme-kuralları)
3. [Dosya ve Namespace Düzeni](#dosya-ve-namespace-düzeni)
4. [Class Yapısı ve Sıralama](#class-yapısı-ve-sıralama)
5. [Fields](#fields)
6. [Events ve Delegates](#events-ve-delegates)
7. [Constructors](#constructors)
8. [Properties](#properties)
9. [Methods](#methods)
10. [Interface Implementation](#interface-implementation)
11. [Region Kullanımı](#region-kullanımı)
12. [Using Direktifleri](#using-direktifleri)
13. [Null Handling](#null-handling)
14. [LINQ Kullanımı](#linq-kullanımı)
15. [Async/Await ve Coroutine](#asyncawait-ve-coroutine)
16. [Exception Handling](#exception-handling)
17. [XML Documentation](#xml-documentation)
18. [Genel Kod Stilleri](#genel-kod-stilleri)
19. [Tam Örnek Class](#tam-örnek-class)
20. [Kontrol Listesi](#kontrol-listesi)

---

## Genel İlkeler

- Kod **okunabilir**, **bakımı kolay** ve **tutarlı** olmalıdır
- **SOLID** prensiplerine uyulmalıdır
- **Magic number/string** kullanılmamalı, `const` veya `readonly` tercih edilmelidir
- Tüm public API'ler **XML documentation** ile belgelenmeli
- **Silent bypass** yapılmamalı, hatalar loglanmalı veya fırlatılmalıdır

### Temel Prensipler

| Prensip | Açıklama |
|---------|----------|
| Okunabilirlik | Kod kendini açıklamalı |
| Tutarlılık | Proje genelinde aynı stil |
| Basitlik | Gereksiz karmaşıklıktan kaçın |
| Performans | Unity main thread kısıtlamalarını göz önünde bulundur |

---

## İsimlendirme Kuralları

### Genel İsimlendirme

| Öğe | Format | Örnek |
|-----|--------|-------|
| Namespace | PascalCase | `MyFeature.Runtime` |
| Class | PascalCase | `PlayerController` |
| Interface | I + PascalCase | `IInteractable` |
| Struct | PascalCase | `DamageInfo` |
| Enum | PascalCase | `PlayerState` |
| Enum Value | PascalCase | `PlayerState.Idle` |
| Method | PascalCase | `TakeDamage()` |
| Property | PascalCase | `Health` |
| Parameter | camelCase | `damageAmount` |
| Local Variable | camelCase | `currentHealth` |

### Field İsimlendirme

| Field Türü | Prefix | Format | Örnek |
|------------|--------|--------|-------|
| public const | - | UPPER_CASE veya PascalCase | `MaxHealth` |
| private const | k_ | PascalCase | `k_MaxHealth` |
| public static | - | PascalCase | `Instance` |
| private static | s_ | PascalCase | `s_Instance` |
| public instance | - | PascalCase | `Health` |
| private instance | m_ | PascalCase | `m_Health` |
| [SerializeField] private | m_ | PascalCase | `m_StartHealth` |

### Örnekler

```csharp
// Constant fields
public const int MaxHealth = 100;
private const int k_MinHealth = 0;

// Static fields
public static PlayerController Instance;
private static int s_PlayerCount;

// Instance fields
public int Health;
private int m_CurrentHealth;

// Serialized fields
[SerializeField] private int m_StartHealth = 100;
```

---

## Dosya ve Namespace Düzeni

### Klasör Yapısı

```
Scripts/
├── Runtime/
│   └── MyFeature/
│       ├── MyClass.cs
│       └── MyStruct.cs
├── Editor/
│   └── MyFeature/
│       ├── MyClassEditor.cs
│       └── MyClassInspector.cs
└── Tests/
    └── MyFeature/
        └── MyClassTests.cs
```

### Namespace Kuralları

| Klasör | Namespace | Açıklama |
|--------|-----------|----------|
| Runtime/ | `MyFeature.Runtime` | Build'e dahil kodlar |
| Editor/ | `MyFeature.Editor` | Sadece Editor'de çalışır |
| Tests/ | `MyFeature.Tests` | Test kodları |

### Dosya Kuralları

| Kural | Açıklama |
|-------|----------|
| Dosya adı = Class adı | `PlayerController.cs` → `class PlayerController` |
| Tek class per dosya | Her dosyada tek bir public class |
| Nested type exception | Nested class'lar ana class dosyasında kalabilir |

```csharp
// PlayerController.cs
namespace GameProject.Runtime
{
    public class PlayerController : MonoBehaviour
    {
        // ...
    }
}
```

---

## Class Yapısı ve Sıralama

### Class İçi Sıralama (Region Sırası)

```
1. Fields
2. Events
3. Constructors
4. Properties
5. Unity Methods (MonoBehaviour ise)
6. Methods
7. Interface Implementations
8. Delegates
9. Nested Types
```

### Erişim Seviyesi Sıralaması

Her bölümde erişim seviyesine göre sıralama:

```
1. public
2. internal
3. protected
4. private
```

### Modifier Sıralaması

Her erişim seviyesinde:

```
1. const
2. static
3. instance (normal)
```

---

## Fields

### Field Sıralaması

```csharp
#region Fields

// Public constant fields
public const string PublicConstant = "Value";

// Internal constant fields
internal const string InternalConstant = "Value";

// Private constant fields
private const string k_PrivateConstant = "Value";

// Public static fields
public static string PublicStaticField;

// Internal static fields
internal static string InternalStaticField;

// Private static fields
private static string s_PrivateStaticField;

// Public instance fields
public string PublicField;

// Internal instance fields
internal string InternalField;

// Serialized private instance fields
[SerializeField] private string m_SerializedField;

// Non-serialized private instance fields
private string m_PrivateField;

#endregion
```

### Field Kuralları

| Kural | Açıklama |
|-------|----------|
| Magic number yasak | `const` veya `readonly` kullan |
| Serialize = private | `[SerializeField]` her zaman `private` ile |
| Public field dikkatli | Mümkünse property tercih et |

```csharp
// Yanlış
private int health = 100;  // Magic number, prefix yok

// Doğru
private const int k_DefaultHealth = 100;
[SerializeField] private int m_Health = k_DefaultHealth;
```

---

## Events ve Delegates

### Event Sıralaması

```csharp
#region Events

// Public static event
public static event Action<int> OnScoreChanged;

// Internal static event
internal static event Action OnGamePaused;

// Private static event
private static event Action s_OnInternalEvent;

// Public instance event
public event Action<DamageInfo> OnDamageTaken;

// Internal instance event
internal event Action OnStateChanged;

// Private instance event
private event Action m_OnPrivateEvent;

#endregion
```

### Event ve Delegate Kuralları

| Durum | Tercih |
|-------|--------|
| Tek parametre | `Action<T>` |
| Parametresiz | `Action` |
| Birden fazla parametre | Custom delegate veya parameter class |
| Return değeri | `Func<T, TResult>` |

```csharp
// Tek parametre - Action kullan
public event Action<int> OnHealthChanged;

// Birden fazla parametre - Parameter class
public event Action<DamageInfo> OnDamageTaken;

public struct DamageInfo
{
    public int Amount;
    public DamageType Type;
    public GameObject Source;
}

// Veya custom delegate
public delegate void DamageHandler(int amount, DamageType type, GameObject source);
public event DamageHandler OnDamageReceived;
```

---

## Constructors

### Constructor Sıralaması

```csharp
#region Constructors

// Public Constructor
public MyClass(int value)
{
    m_Value = value;
}

// Internal Constructor
internal MyClass(string name)
{
    m_Name = name;
}

// Private Constructor (Singleton, Factory)
private MyClass()
{
}

#endregion
```

### Constructor Kuralları

| Kural | Açıklama |
|-------|----------|
| MonoBehaviour | Constructor kullanma, `Awake()` kullan |
| ScriptableObject | Constructor kullanma, `OnEnable()` kullan |
| Normal class | Constructor kullan |
| Dependency Injection | Constructor injection tercih et |

```csharp
// MonoBehaviour - Yanlış
public class PlayerController : MonoBehaviour
{
    public PlayerController() { } // Kullanma!
}

// MonoBehaviour - Doğru
public class PlayerController : MonoBehaviour
{
    private void Awake()
    {
        Initialize();
    }
}

// Normal class - Doğru
public class DamageCalculator
{
    private readonly IWeaponData m_WeaponData;

    public DamageCalculator(IWeaponData weaponData)
    {
        m_WeaponData = weaponData;
    }
}
```

---

## Properties

### Property Türleri ve Kullanımı

| Tür | Kullanım | Örnek |
|-----|----------|-------|
| Auto-property | DTO, basit data class | `public string Name { get; set; }` |
| Expression-bodied | Readonly, hesaplanan değer | `public int Total => m_A + m_B;` |
| Full property | Validation, side-effect | Aşağıdaki örnek |

### Property Kuralları

| Kural | Açıklama |
|-------|----------|
| Unity Serialize | Private field + Public property |
| Auto-property | Mecbur kalmadıkça kullanma |
| Expression-bodied | Readonly için tercih et |

```csharp
#region Properties

// Public Static Property
public static PlayerController Instance { get; private set; }

// Private backing field ile property (Tercih edilen)
public int Health => m_Health;

public int MaxHealth
{
    get => m_MaxHealth;
    set => m_MaxHealth = Mathf.Max(0, value);
}

// Internal Property
internal bool IsAlive => m_Health > 0;

// Private Property
private float HealthPercent => (float)m_Health / m_MaxHealth;

#endregion
```

### Serialize Edilen Alanlar

```csharp
// Yanlış - Public field
public int health = 100;

// Doğru - Private field + Property
[SerializeField] private int m_Health = 100;
public int Health => m_Health;

// Setter gerekiyorsa
public int Health
{
    get => m_Health;
    set => m_Health = Mathf.Clamp(value, 0, m_MaxHealth);
}
```

---

## Methods

### Unity Methods (MonoBehaviour)

```csharp
#region Unity Methods

private void Awake()
{
    Initialize();
}

private void OnEnable()
{
    SubscribeEvents();
}

private void Start()
{
    StartGameplay();
}

private void Update()
{
    HandleInput();
}

private void FixedUpdate()
{
    HandlePhysics();
}

private void LateUpdate()
{
    HandleCamera();
}

private void OnDisable()
{
    UnsubscribeEvents();
}

private void OnDestroy()
{
    Cleanup();
}

#endregion
```

### Unity Method Sıralaması

```
Awake → OnEnable → Start → Update → FixedUpdate → LateUpdate → OnDisable → OnDestroy
```

### Custom Methods Sıralaması

```csharp
#region Methods

// Public Static Method
public static PlayerController FindPlayer()
{
    return Instance;
}

// Internal Static Method
internal static void ResetAll()
{
    s_PlayerCount = 0;
}

// Private Static Method
private static int CalculateScore(int baseScore, float multiplier)
{
    return Mathf.RoundToInt(baseScore * multiplier);
}

// Public Instance Method
public void TakeDamage(int amount)
{
    m_Health -= amount;
    OnDamageTaken?.Invoke(amount);
}

// Internal Instance Method
internal void ResetState()
{
    m_Health = m_MaxHealth;
}

// Private Instance Method
private void HandleDeath()
{
    OnDeath?.Invoke();
    gameObject.SetActive(false);
}

#endregion
```

### Method Kuralları

| Kural | Açıklama |
|-------|----------|
| Tek sorumluluk | Her method tek bir iş yapmalı |
| Kısa tutun | Mümkünse 20-30 satırı geçmesin |
| Anlamlı isim | Ne yaptığı isimden anlaşılmalı |
| Parameter count | Mümkünse 3-4'ü geçmesin |

---

## Interface Implementation

### Explicit vs Implicit

| Tür | Tercih | Açıklama |
|-----|--------|----------|
| Explicit | ✅ Tercih edilir | Interface adı ile çağrılır |
| Implicit | ⚠️ Gerektiğinde | Public method olarak erişilir |

**Kural:** Interface implementation'lar olabildiğince **explicit** olmalıdır.

### Explicit Implementation

```csharp
public class HealthSystem : IDamageable, IHealable
{
    #region Interface Implementations

    // Explicit implementation - Tercih edilen
    int IDamageable.TakeDamage(int amount)
    {
        return ApplyDamage(amount);
    }

    void IHealable.Heal(int amount)
    {
        ApplyHeal(amount);
    }

    #endregion

    // Internal logic
    private int ApplyDamage(int amount)
    {
        m_Health -= amount;
        return amount;
    }

    private void ApplyHeal(int amount)
    {
        m_Health += amount;
    }
}

// Kullanım
IDamageable damageable = healthSystem;
damageable.TakeDamage(10);  // Explicit - interface üzerinden çağrılır

// healthSystem.TakeDamage(10);  // Derleme hatası - doğrudan erişilemez
```

### Implicit Implementation (Gerektiğinde)

```csharp
// Implicit - Public method olarak da erişilebilir olması gerekiyorsa
public class PlayerController : IInteractable
{
    // Implicit implementation
    public void Interact()
    {
        // Hem IInteractable.Interact() hem de PlayerController.Interact() olarak çağrılabilir
    }
}
```

### Ne Zaman Explicit?

| Durum | Implementation |
|-------|----------------|
| Interface sadece belirli sistemler için | Explicit |
| Birden fazla interface aynı method adı | Explicit |
| Internal kullanım | Explicit |
| Public API olarak da gerekli | Implicit |

---

## Region Kullanımı

### Ne Zaman Kullanılmalı?

| Durum | Region |
|-------|--------|
| Kısa class (< 100 satır) | Opsiyonel |
| Orta class (100-300 satır) | Önerilir |
| Büyük class (> 300 satır) | Zorunlu |

### Standart Region'lar

```csharp
#region Fields
#endregion

#region Events
#endregion

#region Constructors
#endregion

#region Properties
#endregion

#region Unity Methods
#endregion

#region Methods
#endregion

#region Interface Implementations
#endregion

#region Delegates
#endregion

#region Nested Types
#endregion
```

### Custom Region'lar

```csharp
#region Input Handling
private void HandleInput() { }
private void ProcessMovement() { }
#endregion

#region Combat System
private void Attack() { }
private void TakeDamage(int amount) { }
#endregion
```

---

## Using Direktifleri

### Sıralama

```csharp
// 1. System namespaces
using System;
using System.Collections;
using System.Collections.Generic;

// 2. Unity namespaces
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 3. Third-party namespaces
using Cysharp.Threading.Tasks;
using DG.Tweening;

// 4. Project namespaces
using MyProject.Runtime;
using MyProject.Runtime.Combat;
```

### Using Kuralları

| Kural | Açıklama |
|-------|----------|
| Alfabetik sıra | Her grup içinde alfabetik |
| Kullanılmayan using | Kaldırılmalı |
| Global using | Proje genelinde ortak using'ler için |

---

## Null Handling

### Unity vs C# Null Check

| Obje Türü | Yöntem | Örnek |
|-----------|--------|-------|
| Unity Object | `== null` | `if (gameObject == null)` |
| C# Object | `?.` ve `??` | `player?.Health ?? 0` |
| UnityEvent | `== null` | `if (onDeath == null)` |

### Unity Object Null Check

```csharp
// Unity Object - == null kullan
if (m_Target == null)
{
    Debug.LogWarning("Target is null!");
    return;
}

// Component reference
if (m_Rigidbody == null)
{
    m_Rigidbody = GetComponent<Rigidbody>();
}
```

### C# Object Null Check

```csharp
// Null conditional operator
int health = player?.Health ?? 0;
player?.TakeDamage(10);

// Null coalescing assignment
m_List ??= new List<int>();

// Pattern matching
if (target is PlayerController player)
{
    player.TakeDamage(damage);
}
```

### Silent Bypass Yasak

```csharp
// Yanlış - Silent bypass
public void Attack(IEnemy target)
{
    if (target == null) return;  // Sessizce geçiyor, hata gizleniyor
    target.TakeDamage(m_Damage);
}

// Doğru - Log veya exception
public void Attack(IEnemy target)
{
    if (target == null)
    {
        Debug.LogError("Attack target is null!");
        return;
    }
    target.TakeDamage(m_Damage);
}

// Veya exception fırlat
public void Attack(IEnemy target)
{
    if (target == null)
    {
        throw new ArgumentNullException(nameof(target));
    }
    target.TakeDamage(m_Damage);
}
```

---

## LINQ Kullanımı

### Genel Kural

| Durum | LINQ |
|-------|------|
| Editor kodu | ✅ Serbestçe kullan |
| Initialization | ✅ Kullan |
| Update/FixedUpdate | ⚠️ Dikkatli kullan |
| Performans kritik | ❌ Kullanma |

### Kabul Edilen Kullanımlar

```csharp
// Initialization'da - OK
private void Awake()
{
    m_Enemies = FindObjectsOfType<Enemy>()
        .Where(e => e.IsActive)
        .OrderBy(e => e.Priority)
        .ToList();
}

// UI sıralaması - OK
public void SortInventory()
{
    m_Items = m_Items
        .OrderBy(item => item.Type)
        .ThenBy(item => item.Name)
        .ToList();
}

// Event handler'da - OK
private void OnEnemyDeath(Enemy enemy)
{
    m_Enemies.RemoveAll(e => e == enemy);

    if (!m_Enemies.Any(e => e.IsAlive))
    {
        OnAllEnemiesDead?.Invoke();
    }
}
```

### Kaçınılması Gereken Kullanımlar

```csharp
// Update'te LINQ - Yanlış
private void Update()
{
    // Her frame allocation yapıyor!
    var nearestEnemy = m_Enemies
        .Where(e => e.IsAlive)
        .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
        .FirstOrDefault();
}

// Update'te - Doğru (Cache kullan)
private Enemy m_CachedNearestEnemy;
private float m_LastSearchTime;

private void Update()
{
    if (Time.time - m_LastSearchTime > k_SearchInterval)
    {
        m_CachedNearestEnemy = FindNearestEnemy();
        m_LastSearchTime = Time.time;
    }
}

private Enemy FindNearestEnemy()
{
    Enemy nearest = null;
    float nearestDistance = float.MaxValue;

    foreach (var enemy in m_Enemies)
    {
        if (!enemy.IsAlive) continue;

        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        if (distance < nearestDistance)
        {
            nearestDistance = distance;
            nearest = enemy;
        }
    }

    return nearest;
}
```

---

## Async/Await ve Coroutine

### UniTask Kullanımı (Tercih Edilen)

```csharp
using Cysharp.Threading.Tasks;

public class AsyncExample : MonoBehaviour
{
    public async UniTaskVoid StartAsync()
    {
        await LoadDataAsync();
        await UniTask.Delay(1000);
        Initialize();
    }

    private async UniTask LoadDataAsync()
    {
        var data = await Resources.LoadAsync<TextAsset>("data");
        ProcessData(data);
    }

    private async UniTask<int> CalculateAsync(int value)
    {
        await UniTask.SwitchToThreadPool();
        int result = HeavyCalculation(value);
        await UniTask.SwitchToMainThread();
        return result;
    }
}
```

### Coroutine Kullanımı

```csharp
public class CoroutineExample : MonoBehaviour
{
    private Coroutine m_CurrentCoroutine;

    public void StartFade()
    {
        if (m_CurrentCoroutine != null)
        {
            StopCoroutine(m_CurrentCoroutine);
        }
        m_CurrentCoroutine = StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < k_FadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = elapsed / k_FadeDuration;
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1f);
    }
}
```

### Async Kuralları

| Kural | Açıklama |
|-------|----------|
| UniTask tercih | Coroutine yerine UniTask kullan |
| Main thread | Unity API main thread'de çağrılmalı |
| Cancellation | Token ile iptal edilebilir yap |
| Fire-and-forget | `UniTaskVoid` kullan |

```csharp
// Cancellation token kullanımı
private CancellationTokenSource m_Cts;

private void OnEnable()
{
    m_Cts = new CancellationTokenSource();
    RunAsync(m_Cts.Token).Forget();
}

private void OnDisable()
{
    m_Cts?.Cancel();
    m_Cts?.Dispose();
}

private async UniTask RunAsync(CancellationToken token)
{
    while (!token.IsCancellationRequested)
    {
        await UniTask.Delay(1000, cancellationToken: token);
        DoSomething();
    }
}
```

---

## Exception Handling

### Temel Kurallar

| Kural | Açıklama |
|-------|----------|
| Boş catch yasak | Her zaman handle et veya logla |
| Spesifik exception | Genel `Exception` yerine spesifik tür |
| Finally kullan | Cleanup için |
| Performans dikkat | Hot path'te try-catch minimumda |

### Doğru Kullanım

```csharp
// Spesifik exception handling
public void LoadFile(string path)
{
    try
    {
        string content = File.ReadAllText(path);
        ProcessContent(content);
    }
    catch (FileNotFoundException ex)
    {
        Debug.LogError($"File not found: {path}. Error: {ex.Message}");
        LoadDefaultContent();
    }
    catch (IOException ex)
    {
        Debug.LogError($"IO error reading file: {path}. Error: {ex.Message}");
        throw; // Re-throw if can't handle
    }
    finally
    {
        CleanupResources();
    }
}
```

### Yanlış Kullanım

```csharp
// Yanlış - Boş catch
try
{
    DoSomething();
}
catch (Exception)
{
    // Sessizce yutma!
}

// Yanlış - Genel exception
try
{
    DoSomething();
}
catch (Exception ex)
{
    Debug.Log(ex); // Çok genel
}
```

### Custom Exception

```csharp
public class GameException : Exception
{
    public GameException(string message) : base(message) { }
    public GameException(string message, Exception inner) : base(message, inner) { }
}

public class InvalidItemException : GameException
{
    public string ItemId { get; }

    public InvalidItemException(string itemId)
        : base($"Invalid item: {itemId}")
    {
        ItemId = itemId;
    }
}
```

---

## XML Documentation

### Zorunlu Documentation

| Öğe | Documentation |
|-----|---------------|
| Public class | ✅ Zorunlu |
| Public method | ✅ Zorunlu |
| Public property | ✅ Zorunlu |
| Public event | ✅ Zorunlu |
| Internal/Private | Opsiyonel |

### Documentation Format

```csharp
/// <summary>
/// Oyuncunun sağlık sistemini yöneten sınıf.
/// </summary>
/// <remarks>
/// Bu sınıf damage, heal ve death işlemlerini yönetir.
/// </remarks>
public class HealthSystem : MonoBehaviour
{
    /// <summary>
    /// Mevcut sağlık değeri.
    /// </summary>
    public int Health => m_Health;

    /// <summary>
    /// Oyuncu öldüğünde tetiklenir.
    /// </summary>
    public event Action OnDeath;

    /// <summary>
    /// Belirtilen miktarda hasar verir.
    /// </summary>
    /// <param name="amount">Verilecek hasar miktarı.</param>
    /// <param name="source">Hasarı veren kaynak.</param>
    /// <returns>Gerçekte verilen hasar miktarı.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="amount"/> negatif olduğunda fırlatılır.
    /// </exception>
    public int TakeDamage(int amount, GameObject source)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Damage cannot be negative.");
        }

        int actualDamage = Mathf.Min(amount, m_Health);
        m_Health -= actualDamage;

        if (m_Health <= 0)
        {
            OnDeath?.Invoke();
        }

        return actualDamage;
    }
}
```

### Yaygın XML Tags

| Tag | Kullanım |
|-----|----------|
| `<summary>` | Kısa açıklama |
| `<remarks>` | Detaylı açıklama |
| `<param>` | Parametre açıklaması |
| `<returns>` | Return değeri açıklaması |
| `<exception>` | Fırlatılabilecek exception |
| `<example>` | Kullanım örneği |
| `<see cref="">` | Referans link |
| `<inheritdoc/>` | Base class doc'u miras al |

---

## Genel Kod Stilleri

### Braces (Süslü Parantez)

```csharp
// Doğru - Yeni satırda (Allman style)
public void Method()
{
    if (condition)
    {
        DoSomething();
    }
}

// Tek satır if - Brace'siz kabul edilir
if (condition)
    DoSomething();

// Yanlış - Tek satırda brace kullanma
if (condition) { DoSomething(); }  // Kullanılmamalı
```

### Spacing

```csharp
// Operatörler etrafında boşluk
int result = a + b;
bool isValid = x > 0 && y < 100;

// Virgülden sonra boşluk
Method(a, b, c);
var list = new List<int> { 1, 2, 3 };

// Parantez içinde boşluk yok
Method(parameter);  // Doğru
Method( parameter ); // Yanlış
```

### Line Length

| Kural | Değer |
|-------|-------|
| Maksimum satır uzunluğu | 120 karakter |
| Tercih edilen | 80-100 karakter |

```csharp
// Uzun satırı böl
var result = SomeVeryLongMethodName(
    firstParameter,
    secondParameter,
    thirdParameter);

// LINQ zinciri
var filtered = items
    .Where(x => x.IsActive)
    .OrderBy(x => x.Name)
    .ToList();
```

### Var Kullanımı

| Durum | var |
|-------|-----|
| Tip açık | ✅ `var player = new PlayerController();` |
| Tip belirsiz | ❌ `int count = GetCount();` |
| LINQ | ✅ `var result = items.Where(...);` |

---

## Tam Örnek Class

```csharp
using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameProject.Runtime
{
    /// <summary>
    /// Oyuncu sağlık sistemini yöneten MonoBehaviour.
    /// </summary>
    [Serializable]
    public class HealthSystem : MonoBehaviour, IDamageable, IHealable
    {
        #region Fields

        // Public constant fields
        public const int MaxHealthLimit = 1000;

        // Private constant fields
        private const int k_MinHealth = 0;
        private const float k_InvincibilityDuration = 0.5f;

        // Private static fields
        private static int s_TotalDamageDealt;

        // Serialized private instance fields
        [SerializeField] private int m_MaxHealth = 100;
        [SerializeField] private int m_StartHealth = 100;
        [SerializeField] private bool m_IsInvincible;

        // Non-serialized private instance fields
        private int m_CurrentHealth;
        private float m_LastDamageTime;

        #endregion

        #region Events

        /// <summary>
        /// Sağlık değiştiğinde tetiklenir.
        /// </summary>
        public event Action<int, int> OnHealthChanged;

        /// <summary>
        /// Hasar alındığında tetiklenir.
        /// </summary>
        public event Action<DamageInfo> OnDamageTaken;

        /// <summary>
        /// Öldüğünde tetiklenir.
        /// </summary>
        public event Action OnDeath;

        #endregion

        #region Properties

        /// <summary>
        /// Mevcut sağlık değeri.
        /// </summary>
        public int CurrentHealth => m_CurrentHealth;

        /// <summary>
        /// Maksimum sağlık değeri.
        /// </summary>
        public int MaxHealth
        {
            get => m_MaxHealth;
            set => m_MaxHealth = Mathf.Clamp(value, 1, MaxHealthLimit);
        }

        /// <summary>
        /// Sağlık yüzdesi (0-1).
        /// </summary>
        public float HealthPercent => (float)m_CurrentHealth / m_MaxHealth;

        /// <summary>
        /// Hayatta mı?
        /// </summary>
        public bool IsAlive => m_CurrentHealth > k_MinHealth;

        /// <summary>
        /// Hasar alabilir mi?
        /// </summary>
        public bool CanTakeDamage => IsAlive && !m_IsInvincible;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            ResetHealth();
        }

        private void OnDisable()
        {
            // Cleanup if needed
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sistemi başlangıç değerleriyle initialize eder.
        /// </summary>
        public void Initialize()
        {
            m_CurrentHealth = m_StartHealth;
        }

        /// <summary>
        /// Sağlığı maksimuma resetler.
        /// </summary>
        public void ResetHealth()
        {
            SetHealth(m_MaxHealth);
        }

        /// <summary>
        /// Belirtilen miktarda hasar verir.
        /// </summary>
        /// <param name="amount">Hasar miktarı.</param>
        /// <returns>Gerçekte verilen hasar.</returns>
        public int TakeDamage(int amount)
        {
            return TakeDamage(amount, null, DamageType.Generic);
        }

        /// <summary>
        /// Detaylı hasar verir.
        /// </summary>
        /// <param name="amount">Hasar miktarı.</param>
        /// <param name="source">Hasar kaynağı.</param>
        /// <param name="type">Hasar türü.</param>
        /// <returns>Gerçekte verilen hasar.</returns>
        public int TakeDamage(int amount, GameObject source, DamageType type)
        {
            if (amount < 0)
            {
                Debug.LogError($"Damage amount cannot be negative: {amount}");
                return 0;
            }

            if (!CanTakeDamage)
            {
                return 0;
            }

            int actualDamage = Mathf.Min(amount, m_CurrentHealth);
            int previousHealth = m_CurrentHealth;

            m_CurrentHealth -= actualDamage;
            m_LastDamageTime = Time.time;
            s_TotalDamageDealt += actualDamage;

            var damageInfo = new DamageInfo
            {
                Amount = actualDamage,
                Source = source,
                Type = type
            };

            OnDamageTaken?.Invoke(damageInfo);
            OnHealthChanged?.Invoke(previousHealth, m_CurrentHealth);

            if (!IsAlive)
            {
                HandleDeath();
            }

            return actualDamage;
        }

        /// <summary>
        /// Belirtilen miktarda iyileştirir.
        /// </summary>
        /// <param name="amount">İyileşme miktarı.</param>
        /// <returns>Gerçekte iyileşen miktar.</returns>
        public int Heal(int amount)
        {
            if (amount < 0)
            {
                Debug.LogError($"Heal amount cannot be negative: {amount}");
                return 0;
            }

            if (!IsAlive)
            {
                return 0;
            }

            int previousHealth = m_CurrentHealth;
            int newHealth = Mathf.Min(m_CurrentHealth + amount, m_MaxHealth);
            int actualHeal = newHealth - m_CurrentHealth;

            m_CurrentHealth = newHealth;
            OnHealthChanged?.Invoke(previousHealth, m_CurrentHealth);

            return actualHeal;
        }

        private void SetHealth(int value)
        {
            int previousHealth = m_CurrentHealth;
            m_CurrentHealth = Mathf.Clamp(value, k_MinHealth, m_MaxHealth);

            if (previousHealth != m_CurrentHealth)
            {
                OnHealthChanged?.Invoke(previousHealth, m_CurrentHealth);
            }
        }

        private void HandleDeath()
        {
            m_CurrentHealth = k_MinHealth;
            OnDeath?.Invoke();
        }

        #endregion

        #region Interface Implementations

        int IDamageable.TakeDamage(int amount)
        {
            return TakeDamage(amount);
        }

        int IHealable.Heal(int amount)
        {
            return Heal(amount);
        }

        #endregion

        #region Nested Types

        /// <summary>
        /// Hasar bilgisi.
        /// </summary>
        public struct DamageInfo
        {
            public int Amount;
            public GameObject Source;
            public DamageType Type;
        }

        /// <summary>
        /// Hasar türleri.
        /// </summary>
        public enum DamageType
        {
            Generic,
            Physical,
            Fire,
            Ice,
            Electric
        }

        #endregion
    }
}
```

---

## Kontrol Listesi

### İsimlendirme

- [ ] Field prefix'leri doğru mu? (m_, s_, k_)
- [ ] PascalCase/camelCase doğru kullanıldı mı?
- [ ] Dosya adı class adı ile aynı mı?

### Yapı

- [ ] Class içi sıralama doğru mu?
- [ ] Erişim seviyesi sıralaması doğru mu? (public → internal → private)
- [ ] Region'lar gerekli yerlerde kullanıldı mı?

### Null Handling

- [ ] Unity object için `== null` kullanıldı mı?
- [ ] Silent bypass yapılmadı mı?
- [ ] Hatalar loglanıyor mu?

### Performans

- [ ] Update'te LINQ kullanımı minimize mi?
- [ ] Magic number/string yok mu?
- [ ] Gereksiz allocation yok mu?

### Documentation

- [ ] Public API'ler XML doc ile belgelendi mi?
- [ ] Parameter ve return açıklamaları var mı?

### Exception

- [ ] Boş catch bloğu yok mu?
- [ ] Spesifik exception kullanıldı mı?

### Interface

- [ ] Interface implementation explicit mi?
- [ ] Implicit sadece gerekli yerlerde mi kullanıldı?

---

## Özet Tablosu

| Konu | Standart |
|------|----------|
| Private field prefix | m_ |
| Private static prefix | s_ |
| Private const prefix | k_ |
| Erişim sırası | public → internal → private |
| Modifier sırası | const → static → instance |
| Property | Private field + Public property |
| Interface | Explicit tercih |
| Tek satır brace | `if (x) { }` kullanılmamalı |
| Unity Object null | `== null` |
| C# Object null | `?.` ve `??` |
| LINQ | Initialization'da OK, Update'te dikkat |
| Async | UniTask tercih |
| Exception | Boş catch yasak, logla |
| XML Doc | Public API zorunlu |

---

*Bu doküman Ludu Arts şirketi içi kullanım için hazırlanmıştır.*
*Sorularınız için Lead Developer ile iletişime geçin.*
