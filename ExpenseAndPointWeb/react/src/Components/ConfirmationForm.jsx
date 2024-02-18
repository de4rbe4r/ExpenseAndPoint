import { React, useState, useEffect } from 'react';
import axios from 'axios';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import { DeleteExpenseUrl} from "../Urls/UrlList";
import Button from 'react-bootstrap/Button';
import AlertModal from './AlertModal.jsx';



const ConfirmationForm = ({ expense, category, isShowForm, setIsShowForm, setIsDataUpdated, isDataUpdated }) => {
    if (expense === undefined && category === undefined) return;
    const [title, setTitle] = useState();
    const [errorMessage, setErrorMessage] = useState("");
    const [showAlertModal, setShowAlertModal] = useState(false);
    const [titleAlert, setTitleAlert] = useState("");


    useEffect(() => {
        if (expense !== undefined) {
            setTitle("Удаление расхода");
        } else {
            setTitle("Удаление категории");
        }
    }, [isShowForm]);


    const handleCloseForm = () => {
        setIsShowForm(false);
    }

    const DeleteExpense = (event) => {
        event.preventDefault();

        if (expense !== undefined) {
            axios.delete(DeleteExpenseUrl + expense.id, expense)
                .then(res => {
                    setShowAlertModal(true);
                    setErrorMessage("Расход успешно удален");
                    setTitleAlert("Успешно");
                })
                .catch(function (error) {
                    if (error.response) {
                        setShowAlertModal(true);
                        setErrorMessage(error.message + ". " + error.response.data.detail);
                        setTitleAlert("Ошибка");
                    }
                });
        }
        // Тут нужно сделать про удаление категории
        else if (action === "Изменить") {
            axios.put(EditExpenseUrl + expense.id, expense)
                .then(res => {
                    setShowAlertModal(true);
                    setErrorMessage("Расход успешно изменен");
                    setTitleAlert("Успешно");
                })
                .catch(function (error) {
                    if (error.response) {
                        setShowAlertModal(true);
                        setErrorMessage(error.message + ". " + error.response.data.detail);
                        setTitleAlert("Ошибка");
                    }
                });
        }
        setIsShowForm(false);
        setIsDataUpdated(!isDataUpdated);
    }

    const setIsShowAlertModal = (data) => {
        setShowAlertModal(data);
    }

    return (
        <>
            <AlertModal showModal={showAlertModal} setIsShowModal={setIsShowAlertModal} errorMessage={errorMessage} title={titleAlert} />
            <Modal
                show={isShowForm}
                onHide={handleCloseForm}
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>{title}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {(
                        (category !== undefined && expense !== undefined) ? (
                            <>
                            <Form.Label htmlFor="category">Категория</Form.Label>
                                <Form.Control
                                    id="category"
                                type="text"
                                placeholder={category.title}
                                readOnly
                                />
                                <Form.Label htmlFor="date">Дата</Form.Label>
                                <Form.Control
                                    id="date"
                                    type="text"
                                    placeholder={expense.dateTime.substr(0, 10)}
                                    readOnly
                                />
                                <Form.Label htmlFor="time">Время</Form.Label>
                                <Form.Control
                                    id="time"
                                    type="text"
                                    placeholder={expense.dateTime.substr(11, 5)}
                                    readOnly
                                />
                                <Form.Label htmlFor="amount">Сумма</Form.Label>
                                <Form.Control
                                    id="amount"
                                    type="text"
                                    placeholder={expense.amount}
                                    readOnly
                                />
                            </>) : 
                            ((category !== undefined) ? (
                                <>
                            <Form.Label htmlFor="category">Категория</Form.Label>
                                <Form.Control
                                    id="category"
                                type="text"
                                placeholder={`Категория: ${category.title}`} 
                                readOnly
                                />
                                </>
                        ) : null)                      
                    ) }
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="danger" onClick={DeleteExpense}>Удалить</Button>
                </Modal.Footer>
            </Modal>
        </>
    )
}

export default ConfirmationForm;