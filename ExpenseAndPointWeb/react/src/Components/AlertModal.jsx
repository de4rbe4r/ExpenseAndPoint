import { React } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';

const AlertModal = ({ showModal, setIsShowModal, errorMessage, title }) => {
    const handleClose = () => setIsShowModal(false);

    return (
        <Modal show={showModal} onHide={handleClose} style={{ backgroundColor: (title === "Ошибка") ? "rgba(255,0,0,0.4)" : "rgba(0,255,0,0.4)" }}>
            <Modal.Header closeButton>
                <Modal.Title >{title}</Modal.Title>
            </Modal.Header>
            <Modal.Body>{errorMessage}</Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Закрыть
                </Button>
            </Modal.Footer>
            </Modal>
    )
}

export default AlertModal;
