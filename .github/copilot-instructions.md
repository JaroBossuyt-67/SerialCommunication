# Copilot instructions for SerialCommunication

Purpose: provide repository-specific guidance for Copilot sessions working on this Windows Forms serial-communication app.

Build / run / test / lint
- Build (Visual Studio / MSBuild):
  - msbuild SerialCommunication.slnx /p:Configuration=Debug
  - or open SerialCommunication.slnx in Visual Studio 2017+ and build.
- Run the built app:
  - bin\Debug\SerialCommunication.exe (after building)
- dotnet CLI: this is a classic, non-SDK .csproj targeting .NET Framework 4.7.2; `dotnet build` may not work reliably.
- Tests: no test projects are present. There is no configured test runner.
- Lint / analyzers: none configured in-repo.

High-level architecture
- Single WinForms desktop application (SerialCommunication namespace).
- Entry point: Program.Main -> launches Form1 (Form1.cs).
- UI and component wiring live in Form1.Designer.cs; Form1.cs contains event handlers and serial logic.
- Serial I/O uses System.IO.Ports.SerialPort (component name: serialPortArduino) configured from UI controls.
- On connect, the app sets PortName/BaudRate/DataBits/Parity/StopBits/Handshake/Rts/Dtr, opens the port, writes "ping", and expects a trimmed "pong" reply before treating the connection as successful.
- Resources (icons/images) live under Resources/ and are embedded via Resources.resx.
- Project is a classic MSBuild project (non-SDK) targeting .NETFramework v4.7.2. Output folders are bin\Debug and bin\Release.

Key repository-specific conventions
- UI control names and labels use Dutch identifiers (e.g., comboBoxPoort, comboBoxBaudrate, radioButtonVerbonden, labelStatus, buttonConnect). Expect variable names in Dutch.
- Default baudrate is selected to "115200" in Form1_Load. To change the default, edit Form1_Load or the Designer default for comboBoxBaudrate.
- Connection handshake: the app depends on a synchronous ping/pong handshake (writes "ping" and reads a line expecting "pong"). When modifying connection code, preserve or document this behavior if it must remain compatible with existing devices.
- Serial operations are performed on the UI thread (synchronous ReadLine/WriteLine). Be cautious when introducing long/blocking operations; migrating to background threads or async patterns will affect UI responsiveness.
- Settings and resources are managed via Properties\Settings.settings and Properties\Resources.resx — use Visual Studio designer for safe edits.

Where to look for common changes
- Add/change serial protocol: Form1.cs (buttonConnect_Click and Form1_Load)
- Change UI or component wiring: Form1.Designer.cs
- Project/build settings: SerialCommunication.csproj
- Embedded resources: SerialCommunication\Resources

AI / assistant config files
- No existing CLAUDE.md, AGENTS.md, .cursorrules, .windsurfrules, CONVENTIONS.md, or similar assistant config files were found. Add repository-specific agent rules here if needed.

Notes for Copilot sessions
- Prefer editing event handlers in Form1.cs and the Designer via Visual Studio to avoid breaking the Designer file structure.
- When suggesting code that touches serial communication, keep the ping/pong handshake and UI control naming in mind to avoid mismatches.
- Avoid converting the project to SDK-style or changing the target framework without explicit user approval.

If you'd like, add repository-specific checks (unit tests, analyzers) or CI steps; tell me where to put them and I can add sample configs.
