package br.com.MJS_API.handlers;

import br.com.MJS_API.exceptions.ErrorResponse;
import br.com.MJS_API.exceptions.ImageNotFoundException;
import br.com.MJS_API.exceptions.PecaNotFoundException;
import br.com.MJS_API.exceptions.PlayerNotFoundException;
import org.apache.coyote.Response;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.context.request.WebRequest;

@ControllerAdvice
public class GlobaExceptionHandler {

    @ExceptionHandler(PlayerNotFoundException.class)
    public ResponseEntity<ErrorResponse> handlePlayerNotFoundException(PlayerNotFoundException ex) {
        ErrorResponse errorResponse = new ErrorResponse("Player not found", ex.getMessage(), HttpStatus.NOT_FOUND.value());
        return new ResponseEntity<>(errorResponse, HttpStatus.NOT_FOUND);
    }

    @ExceptionHandler(PecaNotFoundException.class)
    public ResponseEntity<ErrorResponse> handlePecaNotFoundException(PecaNotFoundException ex) {
        ErrorResponse errorResponse = new ErrorResponse("Peca not found", ex.getMessage(), HttpStatus.NOT_FOUND.value());
        return new ResponseEntity<>(errorResponse, HttpStatus.NOT_FOUND);
    }

    @ExceptionHandler(ImageNotFoundException.class)
    public ResponseEntity<ErrorResponse> handleImageNotFoundException(ImageNotFoundException ex) {
        ErrorResponse errorResponse = new ErrorResponse("Image not found", ex.getMessage(), HttpStatus.NOT_FOUND.value());
        return new ResponseEntity<>(errorResponse, HttpStatus.NOT_FOUND);
    }

    @ExceptionHandler(Exception.class)
    public ResponseEntity<ErrorResponse> handleGlobalExcepetion(Exception ex) {
        ErrorResponse errorResponse = new ErrorResponse("Internal Server Error", ex.getMessage(), HttpStatus.INTERNAL_SERVER_ERROR.value());
        return new ResponseEntity<>(errorResponse, HttpStatus.INTERNAL_SERVER_ERROR);
    }
}
