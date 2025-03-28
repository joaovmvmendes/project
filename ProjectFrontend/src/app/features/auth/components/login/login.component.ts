import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/auth.service';
import { LoginRequest } from '../../../../models/login-request.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    // Inicializa o formulário com os campos username e password
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  // Método que será chamado ao submeter o formulário de login
  onSubmit(): void {
    if (this.loginForm.invalid) {
      return;
    }

    const { username, password } = this.loginForm.value;

    // Cria o objeto LoginRequest com os dados do formulário
    const loginRequest: LoginRequest = { username, password };

    // Envia a solicitação de login para o AuthService
    this.authService.login(loginRequest).subscribe(
      (response) => {
        // Armazena o token no localStorage
        this.authService.storeToken(response.token);
        // Redireciona o usuário para a página de tarefas (ou outra página)
        this.router.navigate(['/tasks']);
      },
      (error) => {
        // Exibe uma mensagem de erro caso o login falhe
        this.errorMessage = 'Usuário ou senha inválidos.';
        console.error('Erro ao autenticar', error);
      }
    );
  }
}
