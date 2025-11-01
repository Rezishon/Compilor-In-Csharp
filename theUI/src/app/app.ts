import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MonacoEditorModule } from '@materia-ui/ngx-monaco-editor';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('theUi');
  editorOptions = { theme: 'vs-dark', language: 'javascript' };
  code: string = 'function x() {\nconsole.log("Hello world!");\n}';
  originalCode: string = 'function x() { // TODO }';
}
