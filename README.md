# Tandm Programming Challenge - Sprinkler System Solution

## Problem Description

Calculate the number of sprinklers required to fill a room, their positions on the ceiling, and connect each sprinkler to the nearest water pipe.

### Input Data

**Ceiling Corners (clockwise):**
| Point | X | Y | Z |
|-------|------|------|------|
| P1 | 97500.01 | 34000.00 | 2500.00 |
| P2 | 85647.67 | 43193.61 | 2500.00 |
| P3 | 91776.75 | 51095.16 | 2530.00 |
| P4 | 103629.07 | 41901.55 | 2530.00 |

**Water Pipes:**
| Pipe | Start (X, Y, Z) | End (X, Y, Z) |
|------|-----------------|---------------|
| P1 | (98242.11, 36588.29, 3000.00) | (87970.10, 44556.09, 3500.00) |
| P2 | (99774.38, 38563.68, 3500.00) | (89502.37, 46531.47, 3000.00) |
| P3 | (101306.65, 40539.07, 3000.00) | (91034.63, 48507.01, 3000.00) |

**Constraints:**
- Sprinklers must be **2500mm from walls**
- Sprinklers must be **2500mm apart** from each other

---

## Mathematical Formulas

### 1. 3D Distance Formula
Used to calculate edge lengths and distances between points:
```
d = √[(x₂-x₁)² + (y₂-y₁)² + (z₂-z₁)²]
```

### 2. Unit Direction Vectors
Calculate normalized direction vectors along ceiling edges:
```
û₁₂ = (P2 - P1) / |P1P2|

unit_12_x = (P2.X - P1.X) / len_12
unit_12_y = (P2.Y - P1.Y) / len_12
```

### 3. Sprinkler Count Calculation
Number of sprinklers along each edge:
```
available_length = edge_length - 2 × margin
count = floor(available_length / spacing) + 1
```

### 4. Grid Position (Vector Addition)
Calculate sprinkler X,Y coordinates using parametric position:
```
X = P1.X + d₁₂ × û₁₂.x + d₁₄ × û₁₄.x
Y = P1.Y + d₁₂ × û₁₂.y + d₁₄ × û₁₄.y

where:
  d₁₂ = distance along edge P1→P2
  d₁₄ = distance along edge P1→P4
```

### 5. Bilinear Interpolation (Z Coordinate)
Handle sloped ceiling by interpolating Z from all 4 corners:
```
Z = (1-u)(1-v)×Z₁ + u(1-v)×Z₂ + uv×Z₃ + (1-u)v×Z₄

where:
  u = d₁₂ / len_12  (parametric position along P1→P2)
  v = d₁₄ / len_14  (parametric position along P1→P4)
```

### 6. Point-to-Line Segment Projection
Find nearest connection point on a pipe:
```
Given: Point P, Pipe segment from A to B

AB = B - A           (pipe direction vector)
AP = P - A           (vector from pipe start to sprinkler)

t = (AP · AB) / (AB · AB)    (projection factor)
t = clamp(t, 0, 1)           (constrain to segment)

Connection = A + t × AB      (nearest point on pipe)
```

---

## Project Structure

```
SprinklerSystem/
├── SprinklerSystem.sln
├── README.md
└── src/
    └── SprinklerSystem/
        ├── Point.cs                    # 3D coordinate (readonly record struct)
        ├── WaterPipe.cs                # Pipe with ID, Start, End points
        ├── SprinklerPlacement.cs       # Result: Position + ConnectionPoint + PipeID
        ├── CeilingGeometryService.cs   # Grid generation & pipe projection logic
        ├── SprinklerLayoutService.cs   # Main calculation orchestrator
        ├── Program.cs                  # Entry point with input data
        └── SprinklerSystem.csproj
```

### Class Descriptions

| Class | Responsibility |
|-------|----------------|
| `Point` | Immutable 3D coordinate with Distance() method |
| `WaterPipe` | Represents a pipe segment with start/end points |
| `SprinklerPlacement` | Holds sprinkler position and its pipe connection |
| `CeilingGeometryService` | Generates grid positions, calculates pipe connections |
| `SprinklerLayoutService` | Orchestrates the calculation workflow |

---

## How to Run

```bash
cd src/SprinklerSystem
dotnet run
```

## Sample Output

**Number of Sprinklers: 15**

| #  | Sprinkler Position (X, Y, Z)              | Connection Point (X, Y, Z)                |
|----|-------------------------------------------|-------------------------------------------|
| 1  | (97056.880990, 37507.646781, 2507.499971) | (97073.578147, 37494.697616, 3056.879416) |
| 2  | (95081.492870, 39039.913656, 2507.499971) | (95101.109615, 39024.703406, 3152.891225) |
| 3  | (93106.104750, 40572.180531, 2507.499971) | (93128.641082, 40554.709196, 3248.903034) |
| 4  | (91130.716630, 42104.447406, 2507.499971) | (91156.172550, 42084.714986, 3344.914844) |
| 5  | (89155.328510, 43636.714280, 2507.499971) | (89183.704017, 43614.720775, 3440.926653) |
| 6  | (98589.140099, 39483.026687, 2514.999942) | (98561.009928, 39504.866576, 3440.938041) |
| 7  | (96613.751979, 41015.293561, 2514.999942) | (96588.540469, 41034.871165, 3344.926186) |
| 8  | (94638.363859, 42547.560436, 2514.999942) | (94616.071009, 42564.875754, 3248.914332) |
| 9  | (92662.975739, 44079.827311, 2514.999942) | (92643.601549, 44094.880343, 3152.902477) |
| 10 | (90687.587620, 45612.094186, 2514.999942) | (90671.132090, 45624.884931, 3056.890623) |
| 11 | (100121.399208, 41458.406592, 2522.499914) | (100121.425767, 41458.440832, 3000.000000) |
| 12 | (98146.011088, 42990.673467, 2522.499914) | (98146.051699, 42990.725821, 3000.000000) |
| 13 | (96170.622969, 44522.940342, 2522.499914) | (96170.677630, 44523.010810, 3000.000000) |
| 14 | (94195.234849, 46055.207217, 2522.499914) | (94195.303562, 46055.295800, 3000.000000) |
| 15 | (92219.846729, 47587.474092, 2522.499914) | (92219.929493, 47587.580789, 3000.000000) |

---

## Visualization

![Sprinkler System Visualization](result.png)

The 3D visualization shows:
- **Cyan surface**: Room ceiling (slightly sloped, Z varies from 2500 to 2530)
- **Black lines**: Three water pipes above the ceiling
- **Red dots**: Sprinkler head positions on the ceiling
- **Red dashed lines**: Connections from each sprinkler to nearest pipe

---

## Algorithm Summary

1. **Calculate ceiling edge lengths** using 3D distance formula
2. **Determine sprinkler grid dimensions** based on margin (2500mm) and spacing (2500mm)
3. **Generate sprinkler positions** using vector addition for X,Y and bilinear interpolation for Z
4. **For each sprinkler**, project onto all pipes and find the nearest connection point
5. **Output** each sprinkler position with its corresponding pipe connection

---

## Technologies

- C# / .NET Core
- No external dependencies
