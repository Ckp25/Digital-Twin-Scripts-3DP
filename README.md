# Unity Digital Twin Scripts for FFF 3D Printer

This repository contains C# scripts developed as part of a Digital Twin implementation for a Fused Filament Fabrication (FFF) 3D printer within the **Autonomous Factory Digital Twin (AFDT)** testbed.

The digital twin was built using **Unity** and synchronizes in real-time with the physical printer over **SSH**, allowing users to remotely monitor telemetry, visualize motion, and trigger key operations like opening the chamber door or starting a print.

>  *This repo includes only the Unity-side scripts. Full Unity project files and hardware backend are not included.*

---

## 🔧 Project Overview

- **Printer Model:** Ikshana FHT-400C (closed-source FFF 3D printer)
- **Connection Protocol:** SSH (via `Renci.SshNet`)
- **Platform:** Unity 2022.x
- **Digital Twin Features:**
  - Real-time telemetry updates (toolhead position, temperatures, humidity, etc.)
  - Smooth animation of extruder, bed, and other moving parts
  - Remote control: chamber door open/close, print start, G-code management
  - Dashboard UI using `TextMeshProUGUI`

---

##  Script Categories

### 1. **Data Acquisition**
- `FetchDataSsh.cs`: Polls telemetry from printer backend via SSH every 0.25 seconds.
- Parses parameters like:
  - PosX, PosY, PosZ
  - TempNoz, TempBed, TempCham
  - FilamentUsed, FilamentWidth, Humidity
  - Status, Speed, Duration

### 2. **Motion and Animation**
- `MoveBedUnity.cs`: Animates the printer bed based on Z-axis position.
- `MoveExtruderUnity.cs`: Animates the toolhead along X/Y axes.
- `MoveBellowHolderUnity.cs`: Animates the cable support assembly.
- All animations use coroutine-based `Lerp()` for smooth motion.

### 3. **Control Triggers**
- `OpenDoorSsh.cs` / `CloseDoorSsh.cs`: Execute scripts to control the chamber door.
- `PrintSsh.cs`: Remotely initiates a print job.
- `AddGcodeSsh.cs` / `GcodeDisplaySsh.cs`: Simulate G-code file operations.

### 4. **Utility**
- `CloseDoorUnity.cs`: Animates door close operation in Unity scene.
- `DashboardUpdater.cs`: Placeholder for central UI update logic.

---

##  Demo Behavior

Upon starting:
1. Scripts establish SSH connection to the printer’s Raspberry Pi.
2. Telemetry is fetched and mapped to dashboard fields.
3. Button presses trigger physical operations and visual animations.
4. All movements are scaled from real-world millimeters to Unity coordinates.

---


##  Thesis Reference

These scripts were developed as part of my Dual Degree Thesis:  
**“Digital Twin for Additive Manufacturing”**  
IIT Madras, 2025 

---

##  Notes

- SSH approach was chosen for prototyping convenience; future upgrades may consider MQTT/WebSockets.
- This project serves as a lightweight alternative to enterprise-grade digital twin solutions.
- Can be extended for closed-loop monitoring, predictive analytics, or full AFDT system coverage.

---

##  Future Work (Suggested)

- Real sensor feedback confirmation (door state, print completion)
- In-app G-code file browser
- Voxel-based print animation
- Predictive print failure detection using telemetry logs

---

##  Requirements

- Unity 2022.x or newer
- [Renci.SshNet](https://github.com/sshnet/SSH.NET) C# library (included via NuGet or DLL)

---

##  License

This code is shared for academic and demonstration purposes. Feel free to fork or adapt with credit.


