import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { MonacoEditorModule } from '@materia-ui/ngx-monaco-editor';
import { FileManagement } from './services/file-management/file-management';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [MonacoEditorModule, ReactiveFormsModule],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App implements OnInit {
  private fileManager = inject(FileManagement);

  codeForm = new FormGroup({
    code: new FormControl(''),
  });

  //TODO: Does file changed since last load or not

  protected readonly title = signal('theUi');

  editorOptions = { theme: 'vs-dark', language: 'shell' };
  originalCode: string = 'function x() { // TODO }';

  async ngOnInit() {
    this.code.setValue((await this.fileManager.getFileCode()).code);
  }

  async onSaveFile() {
    console.log('hello');

    this.fileManager.updateFile(this.code.value);
  }

  get code() {
    return this.codeForm.get('code') as FormControl;
  }
}
